using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PointTowardsTransform : MonoBehaviour
{
    public Transform target;

    [Button]
    private void PointTowards()
    {
        this.transform.LookAt(target);
    }
}
