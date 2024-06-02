using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class ResetCanvasPosition : MonoBehaviour
{
    public Transform cameraTransform;  // Assign your XR camera's transform here in the inspector
    public float distanceFromCamera = 1.25f;  // How far in front of the camera the Canvas should appear
    public float yOffset = 0f;  // Y-axis offset for the canvas position

    [SerializeField] private Transform _companionCanvas1;
    [SerializeField] private Transform _companionCanvas2;
    public Vector3 companionOffset1 = new Vector3(0f, 0f, 0f);  // Example offset
    public Vector3 companionOffset2 = new Vector3(0f, 0f, 0f); // Example offset

    private IEnumerator Start()
    {
        // Wait for a short delay before setting the canvas position
        yield return new WaitForSeconds(0.1f);  // Delay for 1 second, adjust as needed
        ResetCanvas();
    }


    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
        {
            ResetCanvas();
        }
    }

    private void ResetCanvas()
    {
        // Position the Canvas at the camera's position with an offset on the Y-axis
        Vector3 positionOffset = cameraTransform.forward * distanceFromCamera + new Vector3(0, yOffset, 0);
        transform.position = cameraTransform.position + positionOffset;

        // Rotate the Canvas to face directly towards the camera
        Vector3 toCamera = (cameraTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(-toCamera);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }
}
