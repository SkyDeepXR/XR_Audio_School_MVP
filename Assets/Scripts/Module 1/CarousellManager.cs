using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CarousellManager : MonoBehaviour
{
    [SerializeField] private Transform centerPlacement;
    [SerializeField] private Transform preFirstPlacement;
    [SerializeField] private List<Transform> placements;
    [SerializeField] private Transform postLastPlacement;
    
    [SerializeField] private List<GameObject> carousellObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeCarousellObjects();
    }

    private void InitializeCarousellObjects()
    {
        foreach(var obj in carousellObjects)
        {
            obj.transform.position = placements[0].position;
        }

        for (int i = 0; i < carousellObjects.Count; i++)
        {
            carousellObjects[i].SetActive(i < placements.Count);
            
            if (i < placements.Count)
            {
                carousellObjects[i].transform.position = placements[i].position;
                carousellObjects[i].transform.rotation = placements[i].rotation;
            }
        }
    }

    [Button]
    public void GoToPreviousCarousell()
    {
        var newCarousellObjects = new List<GameObject>();

        var lastObject = carousellObjects.Last();
        newCarousellObjects.Add(lastObject);
        lastObject.transform.position = preFirstPlacement.position;
        lastObject.transform.rotation = preFirstPlacement.rotation;
        lastObject.transform.DOMove(placements[newCarousellObjects.Count - 1].position, 0.5f);
        lastObject.transform.DORotateQuaternion(placements[newCarousellObjects.Count - 1].rotation, 0.5f);
        for (int i = 0; i < carousellObjects.Count - 1; i++)
        {
            newCarousellObjects.Add(carousellObjects[i]);
            carousellObjects[i].transform.DOMove(placements[newCarousellObjects.Count - 1].position, 0.5f);
            carousellObjects[i].transform.DORotateQuaternion(placements[newCarousellObjects.Count - 1].rotation, 0.5f);
        }

        carousellObjects = newCarousellObjects;
    }

    [Button]
    public void GoToNextCarousell()
    {
        var newCarousellObjects = new List<GameObject>();
        
        for (int i = 1; i < carousellObjects.Count; i++)
        {
            newCarousellObjects.Add(carousellObjects[i]);
            carousellObjects[i].transform.DOMove(placements[newCarousellObjects.Count - 1].position, 0.5f);
            carousellObjects[i].transform.DORotateQuaternion(placements[newCarousellObjects.Count - 1].rotation, 0.5f);
        }
        
        var firstObject = carousellObjects.First();
        newCarousellObjects.Add(firstObject);
        firstObject.transform.position = postLastPlacement.position;
        firstObject.transform.rotation = postLastPlacement.rotation;
        firstObject.transform.DOMove(placements[newCarousellObjects.Count - 1].position, 0.5f);
        firstObject.transform.DORotateQuaternion(placements[newCarousellObjects.Count - 1].rotation, 0.5f);

        carousellObjects = newCarousellObjects;
    }
    
}
