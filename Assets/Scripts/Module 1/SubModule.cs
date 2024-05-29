using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class SubModule : MonoBehaviour
{
    public UnityEvent OnModuleIntro, OnModuleStart, OnModuleEnd;

    private void OnEnable()
    {
        OnModuleIntro?.Invoke();
    }
    
    [Button]
    public void StartModule()
    {
        OnModuleStart?.Invoke();
    }

    [Button]
    public void CompleteModule()
    {
        OnModuleEnd?.Invoke();
    }
}
