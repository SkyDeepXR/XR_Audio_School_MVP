using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUI_QuizAnswerOption : MonoBehaviour
{
    [SerializeField] private bool isCorrectAnswer;
    public bool IsCorrectAnswer => isCorrectAnswer;
    [SerializeField] private TextMeshProUGUI answerText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string answerTextString)
    {
        answerText.text = answerTextString;
    }

    public void UpdateIsCorrectAnswer(bool val)
    {
        isCorrectAnswer = val;
    }
}
