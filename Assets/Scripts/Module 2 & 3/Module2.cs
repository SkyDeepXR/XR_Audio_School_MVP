using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module2 : MonoBehaviour
{
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private GameObject activitySetup;

    private void OnEnable()
    {
        Reset();
    }
    
    private void Reset()
    {
        //activitySetup.SetActive(false);
    }

    public void StartModule()
    {
        activitySetup.SetActive(true);
    }
}
