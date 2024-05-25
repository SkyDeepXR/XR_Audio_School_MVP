using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubModule : MonoBehaviour
{
    public UnityEvent OnModuleIntro, OnModuleStart;

    private void OnEnable()
    {
        OnModuleIntro?.Invoke();
    }
    
    public void StartModule()
    {
        OnModuleStart?.Invoke();
    }
}
