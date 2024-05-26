using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> modules = new();

    private void Awake()
    {
        // disable all modules
        foreach (var module in modules)
        {
            module.SetActive(false);
        }
    }
    
    public void StartModule(int moduleIndex)
    {
        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].SetActive(i == moduleIndex);
        }
    }
}
