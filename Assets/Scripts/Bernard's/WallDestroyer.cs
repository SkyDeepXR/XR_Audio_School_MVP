using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.Locomotion.Teleporter;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class WallDestroyer : MonoBehaviour
{
    [Serializable]
    public class AudioWithVolume
    {
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f; 
    }
    
    [SerializeField] private float eyePositionOffset = 1.2f;
    [SerializeField] private LayerMask destroyableLayerMask;
    [SerializeField] private List<GameObject> debrisPrefabs;
    [SerializeField] private List<AudioWithVolume> hitSoundsWithVolume;
    [SerializeField] private float[] destroyRadius;
    [SerializeField] private float[] delays;
    [SerializeField] private bool isStopDestroy;

    private Animator _animator;
    private OVRCameraRig _cameraRig;
    private Teleporter _teleporter;
    private int _audioIndex;
    private int _delayIndex;
    private int _destroyRadiusIndex;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }
        _animator.enabled = false;
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
        _animator.enabled = true;
        _delayIndex = 0; // Reset the delay index
        _audioIndex = 0;
        _destroyRadiusIndex = 0;
        StopAllCoroutines();
        StartCoroutine(CallDestroyWallRepeatedly());
        StartCoroutine(StopDestroyAfterInterval());
    }

    IEnumerator StopDestroyAfterInterval()
    {
        yield return new WaitForSeconds(60f);
        isStopDestroy = true;
        _animator.enabled = false;
    }
    private IEnumerator CallDestroyWallRepeatedly()
    {
        DestroyWall();
        Debug.Log($"<color=yellow> destroy wall called Once</color>");
        while (!isStopDestroy)
        {
            Debug.Log($"<color=blue> delay index: {_delayIndex}</color>");
            yield return new WaitForSeconds(delays[_delayIndex]);
            DestroyWall();
            if (_delayIndex < delays.Length - 1)
            {
                _delayIndex++;
            }
        }
    }

    private void DestroyWall()
    {
        Debug.Log($"<color=green> destroy wall called</color>");
        if (Physics.Raycast(transform.position, transform.rotation * Vector3.forward, out RaycastHit hit, Mathf.Infinity, destroyableLayerMask))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.point, destroyRadius[_destroyRadiusIndex], destroyableLayerMask);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Destroy"))
                {
                    if (debrisPrefabs != null && debrisPrefabs.Count > 0)
                    {
                        GameObject debrisPrefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Count)];
                        GameObject debris = Instantiate(debrisPrefab, hit.point, Quaternion.identity);
                        Destroy(hitCollider.gameObject);
                        Destroy(debris, 3f);

                        if (_audioIndex < hitSoundsWithVolume.Count)
                        {
                            AudioWithVolume audioClipWithVolume = hitSoundsWithVolume[_audioIndex];
                            AudioSource.PlayClipAtPoint(audioClipWithVolume.clip, hit.point, audioClipWithVolume.volume);
                            _audioIndex++;
                        }

                        if (_destroyRadiusIndex < destroyRadius.Length - 1)
                        {
                            Debug.Log($"<color=green> radius index: {_destroyRadiusIndex}</color>");
                            _destroyRadiusIndex++;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No debris prefabs assigned or list is empty.");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No object hit by raycast.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 100);
    }
}
