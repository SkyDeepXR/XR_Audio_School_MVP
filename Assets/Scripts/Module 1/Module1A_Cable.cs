using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Module1A_Cable : MonoBehaviour
{
    [SerializeField] private Collider collider;

    private bool isActivated = false;
    public delegate void OnCableActivated(Module1A_Cable cable);
    public OnCableActivated onCableActivated;
    
    [Header("Scale Properties")]
    private float defaultScale;
    [SerializeField] private float scaleWhenActivated;
    private float scaleFadeDuration = 0.5f;

    [Header("UI Elements When Activated")]
    [SerializeField] private CanvasGroup imagePromptCanvasGroup;
    private float imagePromptFadeDuration = 0.5f;
    
    
    [Header("Audio")]
    [SerializeField] private Button replayAudioButton;
    [SerializeField] private AudioClip audioClip;

    [Header("Events")] 
    [SerializeField] private UnityEvent onActivated;
    [SerializeField] private UnityEvent onDeactivated;
    
    

    private void Start()
    {
        defaultScale = transform.localScale.x;

        ToggleImagePrompt(false, 0);
        
        replayAudioButton.onClick.AddListener(PlayAudio);
    }
    
    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    [Button]
    public void Activate()
    {
        onCableActivated?.Invoke(this);
    }

    public void ToggleActiveState(bool val)
    {
        isActivated = val;
        collider.enabled = !isActivated;
        transform.DOScale(Vector3.one * (isActivated ? scaleWhenActivated : defaultScale), scaleFadeDuration);

        ToggleImagePrompt(val, imagePromptFadeDuration);

        replayAudioButton.gameObject.SetActive(isActivated);
        
        if (isActivated)
        {
            onActivated?.Invoke();
            PlayAudio();
        }
        else
        {
            onDeactivated?.Invoke();
        }
    }

    private void ToggleImagePrompt(bool isToggled, float fadeDuration)
    {
        imagePromptCanvasGroup.DOFade(isToggled ? 1 : 0, fadeDuration);
        imagePromptCanvasGroup.interactable = isToggled;
        imagePromptCanvasGroup.blocksRaycasts = isToggled;
    }

    private void PlayAudio()
    {
        // TODO call Module1AudioManager audio function call here
    }
}
