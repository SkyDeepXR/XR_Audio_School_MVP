using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.Locomotion.Teleporter;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallDestroyer : MonoBehaviour
{
    
    [SerializeField] private float eyePositionOffset = 1.2f;
    
    [SerializeField] private float delays;
   
    [SerializeField] private DestroyerGun _gun1;
    [SerializeField] private DestroyerGun _gun2;
    [SerializeField] private DestroyerGun _gun3;
    [SerializeField] private DestroyerGun _gun4;

 
    private OVRCameraRig _cameraRig;
    private Teleporter _teleporter;
    private int count = 1;
    private bool isStopDestroy;
    

    private void Start()
    {
       
        _cameraRig = FindObjectOfType<OVRCameraRig>();
        if (_cameraRig == null)
        {
            Debug.LogError("OVRCameraRig not found in the scene!");
            return;
        }

        _teleporter = FindObjectOfType<Teleporter>();
        if (_teleporter != null)
        {
            _teleporter.enabled = false;
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            StartDestroyingWall();
        }
        
    }

    public void StartDestroyingWall()
    {
        Transform centerEyeAnchor = _cameraRig.centerEyeAnchor;
        
        // Set position and rotation to match the player's gaze direction
        transform.position = centerEyeAnchor.position + centerEyeAnchor.forward * eyePositionOffset;
        transform.rotation = Quaternion.LookRotation(centerEyeAnchor.forward, Vector3.up);
        
        if (_teleporter != null)
        {
            _teleporter.enabled = true;
        }
        StopAllCoroutines();
        StartCoroutine(CallDestroyWallRepeatedly());
    }

  
    private IEnumerator CallDestroyWallRepeatedly()
    {
        
        while (!isStopDestroy)
        {
            Debug.Log($"<color=blue> count value inside while: {count} called</color>");
            yield return new WaitForSeconds(delays);
            if (count == 1)
            {
                _gun1.DestroyWall(count);
                count++;
            }
            else if (count == 2)
            {
                _gun2.DestroyWall(count);
                count++;
            }
            else if (count == 3)
            {
                _gun3.DestroyWall(count);
                count++;
            }
            else if (count == 4)
            {
                _gun4.DestroyWall(count);
                count++;
                isStopDestroy = true;
            }
        }
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 100);
    }
}
