using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Meta.XR.MRUtilityKit;  // Ensure this namespace includes your ObjectModifier class

public class PassthroughViewToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] virtualEnvironmentObjects;

    [SerializeField]
    private OVRPassthroughLayer passthroughLayer;

    [SerializeField]
    private float passthroughAlpha = 1.0f;

    private bool isPassthroughEnabled = true;

    // Reference to ObjectModifier component
    [SerializeField]
    private RoomObjectModifier objectModifier;

    void Start()
    {
        passthroughLayer.textureOpacity = passthroughAlpha;
        Debug.Log($"Start: PassthroughLayer textureOpacity initialized to {passthroughLayer.textureOpacity}");
    }

    public void ToggleView()
    {
        Debug.Log("ToggleView called");
        isPassthroughEnabled = !isPassthroughEnabled;
        Debug.Log($"isPassthroughEnabled set to {isPassthroughEnabled}");

        foreach (var obj in virtualEnvironmentObjects)
        {
            obj.SetActive(!isPassthroughEnabled);
        }

        passthroughLayer.textureOpacity = isPassthroughEnabled ? passthroughAlpha : 0.0f;

        // Toggle Mesh Renderers based on the passthrough state
        if (objectModifier != null)
        {
            objectModifier.ToggleMeshRenderersInMRUKRoom(!isPassthroughEnabled);
        }
        else
        {
            Debug.LogError("ObjectModifier component not found. Make sure it is attached to this GameObject or assigned.");
        }
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            Debug.Log("PassThrough Toggle button pressed.");
            ToggleView();
        }
    }

}