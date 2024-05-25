using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class Mixer : MonoBehaviour
{
    [Header("Mixer Connections")]
    [SerializeField] private SnapInteractable micSocket; 
    [SerializeField] private SnapInteractable speakerSocketL;
    [SerializeField] private SnapInteractable speakerSocketR;
    
    [Space]
    [SerializeField] private InteractableUnityEventWrapper micSocketEventWrapper;
    [SerializeField] private InteractableUnityEventWrapper speakerSocketLEventWrapper;
    [SerializeField] private InteractableUnityEventWrapper speakerSocketREventWrapper;

    [Space]
    [SerializeField, ReadOnly] private CableEnd micCableEnd;
    [SerializeField, ReadOnly] private CableEnd speakerLCableEnd;
    [SerializeField, ReadOnly] private CableEnd speakerRCableEnd;

    [Space]
    [SerializeField, ReadOnly] private SnapInteractable connectedMicSocket;
    [SerializeField, ReadOnly] private SnapInteractable connectedSpeakerSocketL;
    [SerializeField, ReadOnly] private SnapInteractable connectedSpeakerSocketR;

    [Header("Connection Status")]
    [SerializeField, ReadOnly] private bool micConnected;
    [SerializeField] private UnityEvent onMicConnected;
    [SerializeField] private UnityEvent onMicDisconnected;
    [SerializeField, ReadOnly] private bool speakerLConnected;
    [SerializeField] private UnityEvent onSpeakerLConnected;
    [SerializeField] private UnityEvent onSpeakerLDisconnected;
    [SerializeField, ReadOnly] private bool speakerRConnected;
    [SerializeField] private UnityEvent onSpeakerRConnected;
    [SerializeField] private UnityEvent onSpeakerRDisconnected;

    void Awake()
    {
        micSocketEventWrapper.WhenSelect.AddListener(() => 
        {
            var interactor = micSocket.SelectingInteractors.ToList().First();

            if (interactor.TryGetComponent(out micCableEnd))
            {
                connectedMicSocket = micCableEnd.FindConnectingSnapInteractor();
            }
        });
        speakerSocketLEventWrapper.WhenSelect.AddListener(() =>
        {
            var interactor = speakerSocketL.SelectingInteractors.ToList().First();

            if (interactor.TryGetComponent(out speakerLCableEnd))
            {
                connectedSpeakerSocketL = speakerLCableEnd.FindConnectingSnapInteractor();
            }
        });
        speakerSocketREventWrapper.WhenSelect.AddListener(() =>
        {
            var interactor = speakerSocketR.SelectingInteractors.ToList().First();

            if (interactor.TryGetComponent(out speakerRCableEnd))
            {
                connectedSpeakerSocketR = speakerRCableEnd.FindConnectingSnapInteractor();
            }
        });
        
        
        micSocketEventWrapper.WhenUnselect.AddListener(() =>
        {
            micCableEnd = null;
            connectedMicSocket = null;
        });
        speakerSocketLEventWrapper.WhenUnselect.AddListener(() =>
        {
            speakerLCableEnd = null;
            connectedSpeakerSocketL = null;
        });
        speakerSocketREventWrapper.WhenUnselect.AddListener(() =>
        {
            speakerRCableEnd = null;
            connectedSpeakerSocketR = null;
        });
    }

    void Update()
    {
        // connectedMicSocket = micCableEnd? micCableEnd.FindConnectingSnapInteractor() : null;
        // connectedSpeakerSocketL = speakerLCableEnd? speakerLCableEnd.FindConnectingSnapInteractor() : null;
        // connectedSpeakerSocketR = speakerRCableEnd? speakerRCableEnd.FindConnectingSnapInteractor() : null;
        
        UpdateMicConnectionStatus();
        UpdateSpeakerLConnectionStatus();
        UpdateSpeakerRConnectionStatus();
    }

    private void UpdateMicConnectionStatus()
    {
        connectedMicSocket = micCableEnd? micCableEnd.FindConnectingSnapInteractor() : null;
        bool micConnectedNew = micCableEnd != null && connectedMicSocket != null && micCableEnd.FindConnectingSnapInteractor().TryGetComponent(out MicSocket _);

        if (micConnectedNew == micConnected)
            return;

        if (micConnectedNew)
            onMicConnected?.Invoke();
        else
            onMicDisconnected?.Invoke();    
        micConnected = micConnectedNew;
        Debug.Log("Mic Connected: " + micConnected);
    }
    
    private void UpdateSpeakerLConnectionStatus()
    {
        connectedSpeakerSocketL = speakerLCableEnd? speakerLCableEnd.FindConnectingSnapInteractor() : null;
        bool speakerLConnectedNew = speakerLCableEnd != null && connectedSpeakerSocketL != null && speakerLCableEnd.FindConnectingSnapInteractor().TryGetComponent(out SpeakerSocket _);

        if (speakerLConnectedNew == speakerLConnected)
            return;

        if (speakerLConnectedNew)
            onSpeakerLConnected?.Invoke();
        else
            onSpeakerLDisconnected?.Invoke();    
        speakerLConnected = speakerLConnectedNew;
        Debug.Log("Speaker L Connected: " + speakerLConnected);
    }
    
    private void UpdateSpeakerRConnectionStatus()
    {
        connectedSpeakerSocketR = speakerRCableEnd? speakerRCableEnd.FindConnectingSnapInteractor() : null;
        bool speakerRConnectedNew = speakerRCableEnd != null && connectedSpeakerSocketR != null && speakerRCableEnd.FindConnectingSnapInteractor().TryGetComponent(out SpeakerSocket _);

        if (speakerRConnectedNew == speakerRConnected)
            return;

        if (speakerRConnectedNew)
            onSpeakerRConnected?.Invoke();
        else
            onSpeakerRDisconnected?.Invoke();    
        speakerRConnected = speakerRConnectedNew;
        Debug.Log("Speaker R Connected: " + speakerRConnected);
    }
}
