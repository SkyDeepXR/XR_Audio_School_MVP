using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class MicSocket : MonoBehaviour
{
    [SerializeField] private SnapInteractable snapInteractable;
    public bool isConnected => snapInteractable.Interactors.Count > 0;
}
