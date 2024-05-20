using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnapToLocation : MonoBehaviour
{
    public enum SocketType
    {
        XLR_MaleToFemale,
        XLR_FemaleToMale
    }

    [SerializeField] private SocketType _socketType;
    
    
    private bool _grabbed;
    private bool _insideSnapZone;
    public bool snapped;

    [SerializeField] private SnapObject connector;
    [SerializeField] private GameObject _snapRotationReference;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _connectionSound;

    // [Space] 
    // [SerializeField] private SnapObject currentConnector;

    // Detects when connector game object has entered the snap zone radius
    private void OnTriggerEnter(Collider other)
    {
        // if (connector == null || _snapRotationReference == null)
        // {
        //     Debug.LogWarning("Connector or Snap Rotation Reference is not assigned or not spawned yet.");
        //     return;
        // }
        //
        // if (other.gameObject.name == connector.name && !snapped)
        // {
        //     _insideSnapZone = true;
        //     Debug.Log("entered");
        //     SnapObject();
        // }
        
        // NEW
        if (!other.TryGetComponent(out SnapObject snapObject) || _snapRotationReference == null)
            return;

        if (string.Equals(snapObject.connectorType.ToString(), _socketType.ToString() )&& !snapped)
        {
            connector = snapObject;
            _insideSnapZone = true;
            Debug.Log("entered");
            SnapObject();
        }
    }
    // Detects when connector game object has left the snap zone radius
    private void OnTriggerExit(Collider other)
    {
        if (other != null && connector != null && other.gameObject.name == connector.name)
        {
            _insideSnapZone = false;
            Debug.Log("left");
        }
    }
    void SnapObject()
    {
        if (_grabbed == false && _insideSnapZone)
        {
            snapped = true;
        }
    }
    private void Update()
    {
        if (connector == null)
            return;
        
        // Set grabbed to equal the boolean value "isGrabbed" from OVRGrabbable script
        //_grabbed = connector.GetComponent<OVRGrabbable>().isGrabbed; // check for different script
        if (snapped)
        {
            connector.gameObject.transform.position = transform.position;
            connector.gameObject.transform.rotation = _snapRotationReference.transform.rotation;
            //_audioSource.PlayOneShot(_connectionSound[Random.Range(0,2)], .5f);
        }

        if (connector.grabbed)
        {
            snapped = false;
        }
    }
}
