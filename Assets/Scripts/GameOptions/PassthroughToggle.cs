using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class PassthroughViewToggle : MonoBehaviour
{
    // References to the game objects that need to be disabled/enabled
    [SerializeField]
    private GameObject[] virtualEnvironmentObjects;

    // Reference to the OVR Passthrough Layer component
    [SerializeField]
    private OVRPassthroughLayer passthroughLayer;
    
    // The float parameter to be adjusted on the passthrough component
    [SerializeField]
    private float passthroughAlpha = 1.0f;

    // A flag to check if passthrough view is enabled
    private bool isPassthroughEnabled = true;

    void Start()
    {
        // Initialize the passthrough layer opacity based on the initial state
        passthroughLayer.textureOpacity = passthroughAlpha;
        Debug.Log($"Start: PassthroughLayer textureOpacity initialized to {passthroughLayer.textureOpacity}");
    }

    // Function to toggle between passthrough view and virtual environment
    public void ToggleView()
    {
        Debug.Log("ToggleView called");
        isPassthroughEnabled = !isPassthroughEnabled;
        Debug.Log($"isPassthroughEnabled set to {isPassthroughEnabled}");

        // Toggle the game objects
        foreach (var obj in virtualEnvironmentObjects)
        {
            Debug.Log($"Setting {obj.name} active state to {!isPassthroughEnabled}");
            obj.SetActive(!isPassthroughEnabled);
        }

        // Adjust the float parameter on the passthrough component
        passthroughLayer.textureOpacity = isPassthroughEnabled ? passthroughAlpha : 0.0f;
        Debug.Log($"PassthroughLayer textureOpacity set to {passthroughLayer.textureOpacity}");
    }

    void Update()
    {
        // Check if the Y button (Button.Four) is pressed
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            Debug.Log("PassThrough Toggle button pressed. aka Button Number Four as in 4!");
            ToggleView();
        }
    }
}
