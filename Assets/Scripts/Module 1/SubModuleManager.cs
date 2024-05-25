using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubModuleManager : MonoBehaviour
{
    //[SerializeField] private GameObject missionMenu;
    [SerializeField] private List<GameObject> subModules = new();

    private void OnEnable()
    {
        // missionMenu.SetActive(true);
        // // disable all sub-modules
        // for (int i = 0; i < subModules.Count; i++)
        // {
        //     subModules[i].SetActive(false);
        // }

        GoToSubModule(0);
    }

    public void GoToSubModule(int index)
    {
        //missionMenu.SetActive(false);
        for (int i = 0; i < subModules.Count; i++)
        {
            subModules[i].SetActive(i == index);
        }
    }
    

}
