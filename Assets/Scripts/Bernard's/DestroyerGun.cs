using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerGun : MonoBehaviour
{
    [SerializeField] private LayerMask destroyableLayerMask;
    [SerializeField] private List<GameObject> debrisPrefabs;
    [SerializeField] private float destroyRadius;
    [SerializeField] private AudioClip clip;
    [Range(0f, 1f)] [SerializeField] private float volume = 1f;
    
   public void DestroyWall(int count)
    {
        Debug.Log($"<color=green> gun no: {count} called</color>");
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
                        
                            AudioSource.PlayClipAtPoint(clip, hit.point,volume);
                            
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
