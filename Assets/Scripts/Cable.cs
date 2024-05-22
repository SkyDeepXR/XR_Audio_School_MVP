using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

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

    void Awake()
    {
        eventWrapper_MaleEnd.WhenSelect.AddListener(() =>
        {
            connectedMaleSocket = maleEndSnapInteractor.SelectedInteractable;
        });
        eventWrapper_MaleEnd.WhenUnselect.AddListener(() =>
        {
            connectedMaleSocket = null;
        });

        eventWrapper_FemaleEnd.WhenSelect.AddListener(() =>
        {
            connectedFemaleSocket = femaleEndSnapInteractor.SelectedInteractable;
        });
        eventWrapper_FemaleEnd.WhenUnselect.AddListener(() =>
        {
            connectedFemaleSocket = null;
        });
    }
}
