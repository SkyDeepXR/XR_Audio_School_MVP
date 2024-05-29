using System.Collections;
using System.Collections.Generic;
using Meta.XR.Locomotion.Teleporter;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WallDestroyer : MonoBehaviour
{
    [SerializeField] private float eyePositionOffset = 1.2f;
    [SerializeField] private LayerMask destroyableLayerMask;
    [SerializeField] private float destroyRadius = 0.05f;
    [SerializeField] private List<GameObject> debrisPrefabs;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private float[] delays;
    [SerializeField] private bool isStopDestroy;

    private Animator _animator;
    private OVRCameraRig _cameraRig;
    private Teleporter _teleporter;
    private int _audioIndex;
    private int _delayIndex;

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
        
        _animator.enabled = true;
        if (_teleporter != null)
        {
            _teleporter.enabled = true;
        }
        
        isStopDestroy = false;
        _delayIndex = 0; // Reset the delay index
        StopAllCoroutines();
        StartCoroutine(CallDestroyWallRepeatedly());
        StartCoroutine(StopDestroyAfterInterval());
    }

    private IEnumerator StopDestroyAfterInterval()
    {
        yield return new WaitForSeconds(60f);
        isStopDestroy = true;
        _animator.enabled = false;
    }

    private IEnumerator CallDestroyWallRepeatedly()
    {
        DestroyWall();
        while (!isStopDestroy)
        {
            yield return new WaitForSeconds(delays[_delayIndex]);
            DestroyWall();
            if (_delayIndex < delays.Length - 1)
            {
                _delayIndex++;
            }
            destroyRadius += 0.05f;
        }
    }

    private void DestroyWall()
    {
        if (Physics.Raycast(transform.position, transform.rotation * Vector3.forward, out RaycastHit hit, Mathf.Infinity, destroyableLayerMask))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.point, destroyRadius, destroyableLayerMask);
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

                        if (_audioIndex < hitSounds.Length)
                        {
                            AudioSource.PlayClipAtPoint(hitSounds[_audioIndex], hit.point);
                            _audioIndex++; // Move to the next sound
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
