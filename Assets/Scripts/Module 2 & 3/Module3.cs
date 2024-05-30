using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Module3 : MonoBehaviour
{
    public enum Module3GameState
    {
        Intro,
        TransitionedToVR,
        TaskStart,
        TaskFinished,
        PerformanceStart
    }

    [SerializeField, ReadOnly] private Module3GameState gameState;
    [SerializeField] private TaskManager taskManager;

    [Header("Events")] 
    [SerializeField] public UnityEvent OnIntroEvent;
    [SerializeField] public UnityEvent OnTransitionedToVREvent;
    [SerializeField] public UnityEvent OnTaskStartEvent;
    [SerializeField] public UnityEvent OnTaskFailedEvent;
    [SerializeField] public UnityEvent OnTaskFinishedEvent;
    [SerializeField] public UnityEvent OnPerformanceStartEvent;

    void Start()
    {
        taskManager.OnAllTasksCompleted.AddListener(() =>
        {
            GoToState(Module3GameState.TaskFinished);
        });
    }
    
    void OnEnable()
    {
        GoToState(Module3GameState.Intro);
    }

    public void GoToState(int gameStateIndex)
    {
        GoToState((Module3GameState)gameStateIndex);
    }

    public void GoToState(Module3GameState newGameState)
    {
        gameState = newGameState;
        
        switch (gameState)
        {
            case Module3GameState.Intro:
                OnIntroEvent?.Invoke();
                break;
            
            case Module3GameState.TransitionedToVR:
                OnTransitionedToVREvent?.Invoke();
                break;
            
            case Module3GameState.TaskStart:
                OnTaskStartEvent?.Invoke();
                break;
            
            case Module3GameState.TaskFinished:
                OnTaskFinishedEvent?.Invoke();
                break;
            
            case Module3GameState.PerformanceStart:
                OnPerformanceStartEvent?.Invoke();
                break;
        }
    }

    public void VerifyTaskCompletion()
    {
        if (taskManager.allTaskCompleted)
        {
            GoToState(Module3GameState.TaskFinished);
        }
        else
        {
            OnTaskFailedEvent?.Invoke();
        }
    }
    
    
}
