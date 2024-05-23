using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("Canvas Groups")]
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject failurePanel;
    [SerializeField] private TextMeshProUGUI noOfCorrectAnswwersText;
    private const string NO_OF_CORRECT_ANSWERS_TEXT_FORMAT = "Correct Answers: {0}/{1}";
    [SerializeField] private Button retryButton;
    
    [Header("Quiz Questions")]
    [SerializeField] private List<QuizQuestionData> quizQuestions;
    [SerializeField] private int currentQuestionNo;
    
    public delegate void OnQuestionUpdated(QuizQuestionData quizQuestionData, int currentQuestionNo, int totalQuestionNo);
    public OnQuestionUpdated onQuestionUpdated;

    [SerializeField] private int noOfCorrectAnswers;



    private void Awake()
    {
        retryButton.onClick.AddListener(ResetQuiz);
    }
    
    private void Start()
    {
        ResetQuiz();
    }
    
    private void InitializeQuizQuestions(List<QuizQuestionData> newQuizQuestions)
    {
        quizQuestions = newQuizQuestions;
    }

    public void SubmitAnswerAndProceed(bool isCorrectAnswer)
    {
        noOfCorrectAnswers += isCorrectAnswer ? 1 : 0;

        if (currentQuestionNo < quizQuestions.Count - 1)
        {
            ProceedToNextQuestion();
            return;
        }

        if (noOfCorrectAnswers == quizQuestions.Count)
            ShowSuccessPanel();
        else
            ShowFailurePanel();
    }

    private void ShowQuizPanel()
    {
        quizPanel.SetActive(true);
        successPanel.SetActive(false);
        failurePanel.SetActive(false);
    }
    
    private void ShowSuccessPanel()
    {
        quizPanel.SetActive(false);
        successPanel.SetActive(true);
        failurePanel.SetActive(false);
    }
    
    private void ShowFailurePanel()
    {
        quizPanel.SetActive(false);
        successPanel.SetActive(false);
        failurePanel.SetActive(true);

        noOfCorrectAnswwersText.text =
            string.Format(NO_OF_CORRECT_ANSWERS_TEXT_FORMAT, noOfCorrectAnswers, quizQuestions.Count);
    }

    private void ResetQuiz()
    {
        noOfCorrectAnswers = 0;
        ProceedToQuestion(0);
    }

    [Button]
    private void ProceedToNextQuestion()
    {
        ProceedToQuestion(currentQuestionNo + 1);
    }

    private void ProceedToQuestion(int questionNo)
    {
        ShowQuizPanel();
        currentQuestionNo = questionNo;
        onQuestionUpdated?.Invoke(quizQuestions[currentQuestionNo], currentQuestionNo, quizQuestions.Count);
    }
}
