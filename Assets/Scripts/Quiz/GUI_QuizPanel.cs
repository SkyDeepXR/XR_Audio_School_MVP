using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_QuizPanel : MonoBehaviour
{
    [SerializeField] private QuizManager quizManager;
    [Space]
    [SerializeField] private TextMeshProUGUI questionNoText;
    private const string QUESTION_NO_TEXT_FORMAT = "Question {0}/{1}";
    [SerializeField] private TextMeshProUGUI questionText;
    private const string QUESTION_TEXT_FORMAT = "Q. {0}";

    [SerializeField] private Transform answerOptionParent;
    [SerializeField] private RectTransform quizLayoutGroup;
    [SerializeField] private GUI_QuizAnswerOption answerOptionPrefab;
    [SerializeField] private List<GUI_QuizAnswerOption> answerOptions;
    [SerializeField] private ToggleGroup answerOptionToggleGroup;

    [SerializeField] private Button submitButton;

    [SerializeField, ReadOnly] private bool isAnswerSubmitted;
    [SerializeField, ReadOnly] private bool isSubmittedAnswerCorrect;

    private void Awake()
    {
        quizManager.onQuestionUpdated += UpdateQuestion;
        //answerOptionPrefab.gameObject.SetActive(false);
        
        submitButton.onClick.AddListener(SubmitAnswer);
    }

    private void SubmitAnswer()
    {
        quizManager.SubmitAnswerAndProceed(isSubmittedAnswerCorrect);
    }

    private void UpdateQuestion(QuizQuestionData quizQuestionData,
        int currentQuestionNo,
        int totalQuestionNo)
    {
        questionNoText.text =
            string.Format(QUESTION_NO_TEXT_FORMAT, (currentQuestionNo + 1).ToString(), totalQuestionNo.ToString());
        questionText.text = string.Format(QUESTION_TEXT_FORMAT, quizQuestionData.question);

        ClearAnswerOptions();
        
        foreach (var answer in quizQuestionData.wrongAnswers)
        {
            AddAnswerOption(answer, false);
        }
        
        AddAnswerOption(quizQuestionData.correctAnswer, true);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(quizLayoutGroup);
        
        ShuffleAnswerOptions(quizQuestionData.correctAnswerIndex);
        SetToggleGroup();
        ToggleOffAllAnswerOptions();

        SetAnswerSubmitted(false);
        isSubmittedAnswerCorrect = false;
    }

    private void ClearAnswerOptions()
    {
        foreach (var option in answerOptions)
        {
            Destroy(option.gameObject);
        }

        answerOptions.Clear();
    }

    private void AddAnswerOption(string answer, bool isCorrectAnswer)
    {
        var answerOption = Instantiate(answerOptionPrefab, answerOptionParent);
        answerOption.gameObject.SetActive(true);
        answerOption.UpdateIsCorrectAnswer(isCorrectAnswer);
        answerOption.UpdateText(answer);
        answerOption.onAnswerSelected += isCorrect =>
        {
            SetAnswerSubmitted(true);
            isSubmittedAnswerCorrect = isCorrect;
        };
        answerOptions.Add(answerOption);
    }

    [Button]
    private void ShuffleAnswerOptions(int correctAnswerIndex)
    {
        // foreach(var option in answerOptions)
        // {
        //     option.transform.SetSiblingIndex(Random.Range(0, answerOptions.Count));
        // }

        int indexCount = 0;
        GUI_QuizAnswerOption correctAnswerOption = null;
        
        foreach(var option in answerOptions)
        {
            if (option.IsCorrectAnswer)
            {
                correctAnswerOption = option;
                continue;
            }

            option.transform.SetSiblingIndex(indexCount);
            indexCount++;

        }
        
        correctAnswerOption.transform.SetSiblingIndex(correctAnswerIndex);
    }

    private void ToggleOffAllAnswerOptions()
    {
        foreach(var option in answerOptions)
        {
            option.ToggleOff();
        }
    }
    
    private void SetToggleGroup()
    {
        foreach(var option in answerOptions)
        {
            option.SetToggleGroup(answerOptionToggleGroup);
        }
    }
    
    private void SetAnswerSubmitted(bool val)
    {
        isAnswerSubmitted = val;
        submitButton.interactable = isAnswerSubmitted;
    }
}
