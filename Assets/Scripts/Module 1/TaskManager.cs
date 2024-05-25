using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskItem> taskItems;

    public UnityEvent OnAllTasksCompleted;

    public void CompleteTask(int index)
    {
        taskItems[index].CompleteTask();

        if (CheckIfAllTasksAreCompleted())
        {
            OnAllTasksCompleted?.Invoke();
        }
    }

    public void ResetTask(int index)
    {
        taskItems[index].ResetTask();
    }

    private bool CheckIfAllTasksAreCompleted()
    {
        return taskItems.All(t => t.IsCompleted);
    }
}
