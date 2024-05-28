using System.Collections;
using System.Collections.Generic;
using Meta.XR.Locomotion.Teleporter;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class WallDestroyer : MonoBehaviour
{
    [SerializeField] private float eyePositionOffset = 1.2f;
    [SerializeField] private LayerMask destroyableLayerMask;
    [SerializeField] private float destroyRadius = 0.05f;
    [SerializeField] private List<GameObject> debrisPrefabs;
    [SerializeField] private AudioClip hitSound;
     [SerializeField] private bool isStopDestroy;

    private Animator _animator;
    private OVRCameraRig cameraRig;
    private Teleporter _teleporter;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
        _animator.enabled = false;
        
        cameraRig = FindObjectOfType<OVRCameraRig>();
        _teleporter = FindObjectOfType<Teleporter>();

        if (_teleporter != null)
        {
            _teleporter.enabled = false;
        }
        if (cameraRig == null)
        {
            Debug.LogError("OVRCameraRig not found in the scene!");
        }
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            StartDestroyingWall();
        }
    }

   public void StartDestroyingWall()
    {
        Transform centerEyeAnchor = cameraRig.centerEyeAnchor;
        // Set position in front of the player's gaze direction
        transform.position = centerEyeAnchor.position + centerEyeAnchor.forward * eyePositionOffset;

        // Set rotation to match the player's gaze direction
        transform.rotation = Quaternion.LookRotation(centerEyeAnchor.forward, Vector3.up);

        _animator.enabled = true;
        _teleporter.enabled = true;
        
        isStopDestroy = false; // Reset isDestroy in case this method is called again
        StopAllCoroutines(); // Stop any existing coroutines before starting new ones
        StartCoroutine(CallDestroyWallRepeatedly());
        StartCoroutine(StopDestroyAfterInterval());
    }

    IEnumerator StopDestroyAfterInterval()
    {
        yield return new WaitForSeconds(60f);
        isStopDestroy = true;
        _animator.enabled = false;
    }

    IEnumerator CallDestroyWallRepeatedly()
    {
        while (!isStopDestroy)
        {
            yield return new WaitForSeconds(0.9f); 
            DestroyWall(); 
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
                        AudioSource.PlayClipAtPoint(hitSound, hit.point);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.rotation * Vector3.forward * 100);
    }
}
