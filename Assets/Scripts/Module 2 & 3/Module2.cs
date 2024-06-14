using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Module2 : MonoBehaviour
{
    [Header("Equipment Setups")]
    [SerializeField] private GameObject tutorial1Setup;
    [SerializeField] private MicSocket tutorial1MicSocket;
    [SerializeField] private GameObject tutorial2Setup;
    [SerializeField] private GameObject selfTaskAssignmentSetup;
    [SerializeField] private TaskManager selfTaskManager;

    public enum Module2GameState
    {
        Tutorial1Intro,
        Tutorial1,
        Tutorial2Intro,
        Tutorial2,
        TaskIntro,
        TaskStart,
        TaskFinished
    }

    [SerializeField, ReadOnly] private Module2GameState gameState;
    
    [Header("Events")] 
    [SerializeField] public UnityEvent OnTutorial1IntroEvent;
    [SerializeField] public UnityEvent OnTutorial1Event;
    [SerializeField] public UnityEvent OnTutorial2IntroEvent;
    [SerializeField] public UnityEvent OnTutorial2Event;
    [SerializeField] public UnityEvent OnTaskIntroEvent;
    [SerializeField] public UnityEvent OnTaskStartEvent;
    [SerializeField] public UnityEvent OnTaskFailedEvent;
    [SerializeField] public UnityEvent OnTaskFinishedEvent;

    void Update()
    {
        if (gameState == Module2GameState.Tutorial1)
        {
            if (tutorial1MicSocket.isConnected)
                GoToState(Module2GameState.Tutorial2Intro);
        }
    }
    
    private void OnEnable()
    {
        Reset();
    }
    
    private void Reset()
    {
        tutorial1Setup.SetActive(false);
        tutorial2Setup.SetActive(false);
        selfTaskAssignmentSetup.SetActive(false);
    }

    public void GoToState(int gameStateIndex)
    {
        GoToState((Module2GameState) gameStateIndex);
    }
    
    public void GoToState(Module2GameState newGameState)
    {
        gameState = newGameState;

        switch (gameState)
        {
            case Module2GameState.Tutorial1Intro:
                OnTutorial1IntroEvent?.Invoke();
                break;
            
            case Module2GameState.Tutorial1:
                tutorial1Setup.SetActive(true);
                OnTutorial1Event?.Invoke();
                break;
            
            case Module2GameState.Tutorial2Intro:
                OnTutorial2IntroEvent?.Invoke();
                break;
            
            case Module2GameState.Tutorial2:
                // tutorial1Setup.SetActive(false);
                // tutorial2Setup.SetActive(true);
                OnTutorial2Event?.Invoke();
                break;
            
            case Module2GameState.TaskIntro:
                OnTaskIntroEvent?.Invoke();
                break;
            
            case Module2GameState.TaskStart:
                tutorial1Setup.SetActive(false);
                tutorial2Setup.SetActive(false);
                selfTaskAssignmentSetup.SetActive(true);
                OnTaskStartEvent?.Invoke();
                break;
            
            case Module2GameState.TaskFinished:
                OnTaskFinishedEvent?.Invoke();
                Debug.Log("Alll Right you're good at thissssssssssssss   Successful completion of module 2 has happened  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                break;
        }
    }

    [Button]
    public void GoToNextGameState()
    {
        GoToState(gameState + 1);
    }
    
    public void VerifyTaskCompletion()
    {
        if (selfTaskManager.allTaskCompleted)
        {
            GoToState(Module2GameState.TaskFinished);
        }
        else
        {
            OnTaskFailedEvent?.Invoke();
        }
    }
}
