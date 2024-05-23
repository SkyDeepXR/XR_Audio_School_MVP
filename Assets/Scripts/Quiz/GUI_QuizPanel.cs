using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_QuizPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionNoText;
    private const string QUESTION_NO_TEXT_FORMAT = "Question {0}/{1}";
    [SerializeField] private TextMeshProUGUI questionText;
    private const string QUESTION_TEXT_FORMAT = "Q. {0}";

    [SerializeField] private GUI_QuizAnswerOption answerOptionPrefab;
    [SerializeField] private List<GUI_QuizAnswerOption> answerOptions;

    [SerializeField] private Button submitButton; 

    private void UpdateQuestion(int currentQuestionNo,
        int totalQuestionNo,
        QuizQuestionData quizQuestionData)
    {
        questionNoText.text =
            string.Format(QUESTION_NO_TEXT_FORMAT, currentQuestionNo.ToString(), totalQuestionNo.ToString());
        questionText.text = string.Format(QUESTION_TEXT_FORMAT, quizQuestionData.question);

        ClearAnswerOptions();

        AddAnswerOption(quizQuestionData.correctAnswer, true);
        
        foreach (var answer in quizQuestionData.wrongAnswers)
        {
            AddAnswerOption(answer, false);
        }
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
        var answerOption = Instantiate(answerOptionPrefab, transform);
        answerOption.UpdateIsCorrectAnswer(isCorrectAnswer);
        answerOption.UpdateText(answer);
        answerOptions.Add(answerOption);
    }
}
