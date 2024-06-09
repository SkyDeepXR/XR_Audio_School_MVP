using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using Obi;

public class RoomObjectModifier : MonoBehaviour
{
    public ObiCollisionMaterial frictionMaterial;

    public void GetRoomObjectAndDestroy()
    {
        MRUKRoom mrukComponent = FindObjectOfType<MRUKRoom>();
        if (mrukComponent != null)
        {
            GameObject mrukObject = mrukComponent.gameObject;
            DestroyObjectHierarchy(mrukObject);
        }
    }

    private void DestroyObjectHierarchy(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            DestroyObjectHierarchy(child.gameObject);
        }
        Destroy(obj);
    }

    public void ToggleMeshRenderersInMRUKRoom(bool enableRenderers)
    {
        MRUKRoom mrukComponent = FindObjectOfType<MRUKRoom>();
        if (mrukComponent != null)
        {
            ToggleMeshRenderers(mrukComponent.gameObject, enableRenderers);
        }
    }

    private void ToggleMeshRenderers(GameObject obj, bool enableRenderers)
    {
        foreach (Transform child in obj.transform)
        {
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = enableRenderers;
            }
            // Recursively toggle renderers in deeper child objects
            ToggleMeshRenderers(child.gameObject, enableRenderers);
        }
    }

    public void AddObiCollidersToRoomObjects()
    {
        MRUKRoom mrukComponent = FindObjectOfType<MRUKRoom>();
        if (mrukComponent != null)
        {
            AddObiColliders(mrukComponent.gameObject);
        }
    }

    private void AddObiColliders(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                ObiCollider obiCollider = child.gameObject.AddComponent<ObiCollider>();
                obiCollider.CollisionMaterial = frictionMaterial;
            }
            // Recursively add ObiColliders in deeper child objects
            AddObiColliders(child.gameObject);
        }
    }
}
