using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsSpawnTrigger : MonoBehaviour
{
   
   private VRPropsManager _vrPropsManager;

   private void Start()
   {
       _vrPropsManager = FindObjectOfType<VRPropsManager>();
   }

   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_vrPropsManager != null)
            {
                _vrPropsManager.ActivateProps();
            }
            else
            {
                Debug.LogError("Have you drag VR Props Manager into the scene?");
            }
        }
    }
}
