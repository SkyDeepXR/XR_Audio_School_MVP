using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class CableEnd : MonoBehaviour
{
    [SerializeField] private SnapInteractor snapInteractor;
    [SerializeField] private Cable _cable;
    public Cable cable => _cable;

    public SnapInteractable FindConnectingSnapInteractor()
    {
        return _cable.FindConnectingSnapInteractable(snapInteractor);
    }
}
