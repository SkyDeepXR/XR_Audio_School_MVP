using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using Obi;

public class RoomObjectModifier : MonoBehaviour
{
    

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
}