using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SimplePrefabSpawner : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    
    [System.Serializable]
    public struct PrefabPair
    {
        public GameObject prefab;
        public GameObject previewPrefab;
    }

    public List<PrefabPair> prefabPairs;
    private GameObject _currentPreview;
    private int _currentPrefabIndex = 0; // Example index to track which prefab is selected

    private float _cumulativeYRotation = 0f;
    
    public bool canPlaceObjects = false;

    
    
    void Awake()
    {
        //_oVRSceneManager = FindObjectOfType<OVRSceneManager>();
        //_oVRSceneManager.SceneModelLoadedSuccessfully += SceneLoaded;
    }
    
    void Start()
    {
        canPlaceObjects = false;
        if (prefabPairs.Count > 0)
            _currentPreview = Instantiate(prefabPairs[_currentPrefabIndex].previewPrefab);
        
    }
    
    public void EnablePlacing()
    {
        canPlaceObjects = true;
    }

    public void DisablePlacing()
    {
        canPlaceObjects = false;
    }
    
    void Update()
    {
        if (!canPlaceObjects) return;
        
        Ray ray = new Ray(
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (_currentPreview != null)
            {
                _currentPreview.transform.position = hit.point;
                _currentPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                _cumulativeYRotation += thumbstickInput.x * Time.deltaTime * 50;
                _currentPreview.transform.rotation *= Quaternion.Euler(0, _cumulativeYRotation, 0);
            }

            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                InstantiateCurrentPrefab(hit.point);
                UpdatePreviewToNextPrefab();
            }
        }
    }

    void InstantiateCurrentPrefab(Vector3 position)
    {
        Instantiate(prefabPairs[_currentPrefabIndex].prefab, position, _currentPreview.transform.rotation);
    }

    /* void UpdatePreviewToNextPrefab()
    {
        Destroy(_currentPreview); // Destroy current preview
        _currentPrefabIndex = (_currentPrefabIndex + 1) % prefabPairs.Count; // Cycle through the list

        if (prefabPairs.Count > _currentPrefabIndex) // Check if there's another prefab to preview
        {
            _currentPreview = Instantiate(prefabPairs[_currentPrefabIndex].previewPrefab);
        }
    }*/
    
    void UpdatePreviewToNextPrefab()
    {
        Destroy(_currentPreview);
        _currentPrefabIndex = (_currentPrefabIndex + 1) % prefabPairs.Count;

        if (_currentPrefabIndex == 0) // All objects have been cycled through
        {
            //FindObjectOfType<UIManager>().EndPlacing();
        }
        else
        {
            _currentPreview = Instantiate(prefabPairs[_currentPrefabIndex].previewPrefab);
        }
    }
    
    
    
}
