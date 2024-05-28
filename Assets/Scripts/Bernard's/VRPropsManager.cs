using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class VRPropsManager : MonoBehaviour
{
    private GameObject[] props;
    private MRUKRoom roomMesh;

    // Start is called before the first frame update
    void Start()
    {
        roomMesh = FindObjectOfType<MRUKRoom>();
        if (roomMesh == null)
        {
            Debug.LogError("MRUKRoom not found!");
        }

        props = GameObject.FindGameObjectsWithTag("Props");
        
        // Delay setting all props inactive to ensure it's the last operation
        StartCoroutine(SetPropsInactiveAfterDelay());
    }

    private IEnumerator SetPropsInactiveAfterDelay()
    {
        yield return new WaitForSeconds(0.001f); // Adjust the delay as needed

        foreach (GameObject prop in props)
        {
            prop.SetActive(false);
        }
    }

    public void ActivateProps()
    {
        foreach (GameObject prop in props)
        {
            prop.SetActive(true);
        }
        
        if (roomMesh != null)
        {
            roomMesh.gameObject.SetActive(false);
        }
    }
    
}
