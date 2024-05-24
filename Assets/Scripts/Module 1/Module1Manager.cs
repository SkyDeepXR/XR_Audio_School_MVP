using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module1Manager : MonoBehaviour
{
    [SerializeField] private List<GameObject> subModules = new();

    private void Start()
    {
        GoToSubModule(0);
    }

    public void GoToSubModule(int index)
    {
        for (int i = 0; i < subModules.Count; i++)
        {
            subModules[i].SetActive(i == index);
        }
    }
    

}
