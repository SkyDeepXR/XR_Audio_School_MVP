using UnityEngine;

public class RoomDimensions : MonoBehaviour
{
    void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds combinedBounds = renderers[0].bounds;
            foreach (Renderer renderer in renderers)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }

            Vector3 roomSize = combinedBounds.size;
            Debug.Log("Room Dimensions:");
            Debug.Log("Width: " + roomSize.x);
            Debug.Log("Height: " + roomSize.y);
            Debug.Log("Depth: " + roomSize.z);
        }
        else
        {
            Debug.LogError("No Renderer components found in the GameObject or its children.");
        }
    }
}