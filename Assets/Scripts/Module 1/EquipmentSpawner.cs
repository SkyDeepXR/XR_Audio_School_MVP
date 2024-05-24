using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EquipmentSpawner : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Vector3 spawnOffsetFromCamera;

    [Space] 
    [SerializeField] private TaskManager_1B taskManager;
    [SerializeField] private List<GameObject> equipmentsToSpawn;

    private int noOfEquipmentsPlaced;

    private void Awake()
    {
        mainCamera = Camera.main;
        
        foreach (var equipment in equipmentsToSpawn)
        {
            equipment.SetActive(false);
        }
    }

    private void Start()
    {
        Invoke(nameof(Reset), 0.5f);
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && noOfEquipmentsPlaced < equipmentsToSpawn.Count)
        {
            taskManager.CompleteTask(noOfEquipmentsPlaced);
            noOfEquipmentsPlaced++;
            SpawnEquipment(noOfEquipmentsPlaced);
        }
    }

    private void SpawnEquipment(int index)
    {
        if (index >= equipmentsToSpawn.Count)
            return;
        
        var currentEquipmentToSpawn = equipmentsToSpawn[index];
        
        currentEquipmentToSpawn.SetActive(true);
        currentEquipmentToSpawn.transform.parent = null;    // unlink to ResetCanvasPosition.cs

        currentEquipmentToSpawn.transform.position = mainCamera.transform.position +
                                                     mainCamera.transform.right * spawnOffsetFromCamera.x +
                                                     mainCamera.transform.up * spawnOffsetFromCamera.y +
                                                     mainCamera.transform.forward * spawnOffsetFromCamera.z;
        currentEquipmentToSpawn.transform.LookAt(mainCamera.transform);
        var rot = currentEquipmentToSpawn.transform.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        currentEquipmentToSpawn.transform.eulerAngles = rot;
        
        // replace spawn fx here
        currentEquipmentToSpawn.transform.localScale = Vector3.zero;
        currentEquipmentToSpawn.transform.DOScale(Vector3.one, 0.1f);
    }

    private void Reset()
    {
        foreach (var equipment in equipmentsToSpawn)
        {
            equipment.SetActive(false);
        }

        noOfEquipmentsPlaced = 0;
        
        SpawnEquipment(0);
    }
}
