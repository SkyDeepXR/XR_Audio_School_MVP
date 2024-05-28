using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module1A_Manager : MonoBehaviour
{
    [SerializeField] private List<Module1A_Cable> cables;

    void Awake()
    {
        foreach (Module1A_Cable cable in cables)
        {
            cable.onCableActivated += ActivateCable;
        }
    }
    
    private void ActivateCable(Module1A_Cable cableToActivate)
    {
        foreach (var cable in cables)
        {
            cable.ToggleActiveState(cable == cableToActivate);
        }
    }
}
