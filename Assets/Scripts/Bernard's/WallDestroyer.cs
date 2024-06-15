using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.Locomotion.Teleporter;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
class  Guns
{
    public DestroyerGun gun;
   public float delay;
}
public class WallDestroyer : MonoBehaviour
{
    
    [SerializeField] private float eyePositionOffset = 1.2f;
    
    [Space] [SerializeField] private List<Guns> _gunsList;
 
    private OVRCameraRig _cameraRig;
    private Teleporter _teleporter;
    private int _gunIndex = 0;
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
            Debug.Log($"<color=blue> index value inside while: {_gunIndex} called</color>");
            yield return new WaitForSeconds(_gunsList[_gunIndex].delay);
            _gunsList[_gunIndex].gun.DestroyWall(_gunIndex);
            _gunIndex++;

           
            if (_gunIndex == _gunsList.Count)
            {
                isStopDestroy = true;
            }
        }
    }
    
}
