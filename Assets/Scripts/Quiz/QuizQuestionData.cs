using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizQuestionData", menuName = "Audio School XR/Quiz Question Data", order = 1)]
public class QuizQuestionData : ScriptableObject
{
    public string question;
    public string correctAnswer;
    public List<string> wrongAnswers;
    public int correctAnswerIndex;
}
