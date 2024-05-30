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
        props = GameObject.FindGameObjectsWithTag("Props");
        
        foreach (GameObject prop in props)
        {
            prop.SetActive(false);
        }
    }

    public void OnFindRoomMesh()
    {
        roomMesh = FindObjectOfType<MRUKRoom>();
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
