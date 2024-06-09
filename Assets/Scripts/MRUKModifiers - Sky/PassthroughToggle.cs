using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Meta.XR.MRUtilityKit;
using UnityEngine.Serialization;

public class PassthroughViewToggle : MonoBehaviour
{
    [SerializeField] private GameObject[] virtualEnvironmentObjects;
    [SerializeField] private OVRPassthroughLayer passthroughLayer;
    [SerializeField] private float passthroughAlpha = 1.0f;
    private bool isPassthroughEnabled = true;
    [SerializeField] private RoomObjectModifier objectModifier;
    [SerializeField] private GameObject _avatarEntity;
    [SerializeField] private GameObject _avatarManager;

    // List of materials to modify
    [SerializeField] private List<Material> materialsToModify;

    // UnityEvent for toggle action
    [SerializeField] private UnityEvent OnToggle;

    private bool initializationComplete = false;

    void Start()
    {
        passthroughLayer.textureOpacity = passthroughAlpha;
        Debug.Log($"Start: PassthroughLayer textureOpacity initialized to {passthroughLayer.textureOpacity}");
        StartCoroutine(InitializeVirtualEnvironment());
    }

    private IEnumerator InitializeVirtualEnvironment()
    {
        yield return new WaitForSeconds(5.0f);
        ToggleView(false); // Start with passthrough view disabled
        ToggleView(false); // Allow initial toggle to passthrough view
        initializationComplete = true;
    }

    public void ToggleView(bool triggerEvent = true)
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

        // Update _avatarEntity activation based on passthrough state
        _avatarEntity.SetActive(!isPassthroughEnabled);

        // Change the render queue of materials
        SetRenderQueue(isPassthroughEnabled ? 3000 : 1000);

        // Trigger the OnToggle event if initialization is complete and triggering event is allowed
        if (initializationComplete && triggerEvent)
        {
            OnToggle.Invoke();
        }
    }

    private void SetRenderQueue(int renderQueue)
    {
        foreach (var material in materialsToModify)
        {
            if (material != null)
            {
                material.renderQueue = renderQueue;
                Debug.Log($"Set render queue for {material.name} to {renderQueue}");
            }
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
