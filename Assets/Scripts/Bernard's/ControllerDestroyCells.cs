using UnityEngine;
using System.Collections.Generic;

namespace BlackWhale.DestructibleMeshSystem.Demo
{
    public class ControllerDestroyCells : MonoBehaviour
    {
        [SerializeField] private LayerMask destroyableLayerMask;
        [SerializeField] private List<GameObject> debrisPrefabs;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private float destroyRadius = 0.05f;

        private Vector3 lastRayOrigin;
        private Vector3 lastRayDirection;

        void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                TryDestroyObjectInPath();
            }
        }

        private void TryDestroyObjectInPath()
        {
            Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            Vector3 rayDirection = controllerRotation * Vector3.forward;

            lastRayOrigin = controllerPosition;
            lastRayDirection = rayDirection;

            if (Physics.Raycast(controllerPosition, rayDirection, out RaycastHit hit, Mathf.Infinity, destroyableLayerMask))
            {
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, destroyRadius, destroyableLayerMask);
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Destroy"))
                    {
                        GameObject debrisPrefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Count)];
                        GameObject debris = Instantiate(debrisPrefab, hit.point, Quaternion.identity);
                        Destroy(hitCollider.gameObject);
                        Destroy(debris, 3f);

                        AudioSource.PlayClipAtPoint(hitSound, hit.point);
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (lastRayOrigin != null && lastRayDirection != null)
            {
                Gizmos.DrawRay(lastRayOrigin, lastRayDirection * 100);
            }
        }
    }
}