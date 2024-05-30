using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_QuizAnswerOption : MonoBehaviour
{
    [SerializeField] private bool isCorrectAnswer;
    public bool IsCorrectAnswer => isCorrectAnswer;
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private Toggle toggle;
    public bool IsSelected => toggle.isOn;
    
    public delegate void OnAnswerSelected(bool isCorrectAnswer);
    public OnAnswerSelected onAnswerSelected;

    [Header("DEBUG")]
    [SerializeField] private int siblingIndex_correct;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(isOn =>
        {
            if (!isOn) return;
            
            onAnswerSelected?.Invoke(isCorrectAnswer);
        });
    }

    public void UpdateText(string answerTextString)
    {
        answerText.text = answerTextString;
    }

    public void UpdateIsCorrectAnswer(bool val)
    {
        isCorrectAnswer = val;
    }

    public void ToggleOff()
    {
        toggle.isOn = false;
    }

    public void SetToggleGroup(ToggleGroup toggleGroup)
    {
        toggle.group = toggleGroup;
    }

    [Button]
    private void SetSiblingIndex()
    {
        transform.SetSiblingIndex(siblingIndex_correct);
    }
    
    public void SetSiblingIndex(int index)
    {
        siblingIndex_correct = index;
        transform.SetSiblingIndex(index);
    }
}
