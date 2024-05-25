using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Oculus.Interaction;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    [Header("Mixer Connection")]
    [SerializeField] private SnapInteractable xlrSocket;
    [SerializeField] private InteractableUnityEventWrapper xlrSocketEvnetWrapper;
    
    //[SerializeField] private SnapInteractor selectedInteractor;
    [SerializeField, ReadOnly] private CableEnd connectedXlrCableEnd;
    [SerializeField, ReadOnly] private SnapInteractable connectedXlrSocket;

    void Awake()
    {
        xlrSocketEvnetWrapper.WhenSelect.AddListener(() =>
        {
            var selectedInteractor = xlrSocket.SelectingInteractors.ToList().First();

            if (selectedInteractor.TryGetComponent(out connectedXlrCableEnd))
            {
                connectedXlrSocket = connectedXlrCableEnd.FindConnectingSnapInteractor();
            }
        });
        xlrSocketEvnetWrapper.WhenUnselect.AddListener(() =>
        {
            //connectedXlrCable.OnCableConnectionChanged.RemoveListener(UpdateConnectedXlrDevice);
            connectedXlrCableEnd = null;
            connectedXlrSocket = null;
        });
    }

    void Update()
    {
        connectedXlrSocket = connectedXlrCableEnd ? connectedXlrCableEnd.FindConnectingSnapInteractor() : null;
    }
}
