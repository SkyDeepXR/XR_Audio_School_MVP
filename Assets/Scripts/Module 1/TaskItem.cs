using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    public bool IsCompleted => toggle.isOn;

    public void CompleteTask()
    {
        toggle.isOn = true;
    }

    public void ResetTask()
    {
        toggle.isOn = false;
    }
}
