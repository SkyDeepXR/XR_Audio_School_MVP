using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class Cable : MonoBehaviour
{
    [Header("Male End")]
    [SerializeField] private SnapInteractor maleEndSnapInteractor;
    [SerializeField, ReadOnly] private SnapInteractable connectedMaleSocket;
    [SerializeField] private InteractorUnityEventWrapper eventWrapper_MaleEnd;
    
    [Header("Female End")]
    [SerializeField] private SnapInteractor femaleEndSnapInteractor;
    [SerializeField, ReadOnly] private SnapInteractable connectedFemaleSocket;
    [SerializeField] private InteractorUnityEventWrapper eventWrapper_FemaleEnd;

    public UnityEvent OnCableConnectionChanged;
    
    void Awake()
    {
        eventWrapper_MaleEnd.WhenSelect.AddListener(() =>
        {
            connectedMaleSocket = maleEndSnapInteractor.SelectedInteractable;
            OnCableConnectionChanged?.Invoke();
        });
        eventWrapper_MaleEnd.WhenUnselect.AddListener(() =>
        {
            connectedMaleSocket = null;
            OnCableConnectionChanged?.Invoke();
        });

        eventWrapper_FemaleEnd.WhenSelect.AddListener(() =>
        {
            connectedFemaleSocket = femaleEndSnapInteractor.SelectedInteractable;
            OnCableConnectionChanged?.Invoke();
        });
        eventWrapper_FemaleEnd.WhenUnselect.AddListener(() =>
        {
            connectedFemaleSocket = null;
            OnCableConnectionChanged?.Invoke();
        });
    }

    public SnapInteractable FindConnectingSnapInteractable(SnapInteractor socket)
    {
        if (socket == maleEndSnapInteractor)
        {
            return connectedFemaleSocket;
        }
        else if (socket == femaleEndSnapInteractor)
        {
            return connectedMaleSocket;
        }

        return null;
    }

}
