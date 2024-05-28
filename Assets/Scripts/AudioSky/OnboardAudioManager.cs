using System;
using System.Collections;
using UnityEngine;
using SonicBloom.Koreo;
using DG.Tweening;

public class OnboardAudioManager : MonoBehaviour
{
    public static OnboardAudioManager instance;

    [Header("VOICEOVER")]
    // Dedicated AudioSource for voiceover

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioClip _bgMusic;
    
    [SerializeField] private AudioSource _onboardSource;
    [SerializeField] AudioClip[] _onboardClips;  // Audio clips for Onboarding Voiceover
    [SerializeField] private NarrationAudioEvent[] _narrationAudioEvents;
    [SerializeField] private AudioClip[] _pressButtonVO;

    private bool _audioSkipped = true;
    
    // Use this for initialization
    void Start()
    {
        //Koreographer.Instance.RegisterForEvents("TestEventID", FireEventDebugLog);
    }
    void FireEventDebugLog(KoreographyEvent koreoEvent)
    {
        Debug.Log("Koreography Event Fired.");
    }

    
    public void PlayNarrationClip(int clipIndex, Action onAudioEndCallback = null)  // Play narration clip according to their index number. Call this from an Event Manager.
    {
        if (clipIndex < 0 || clipIndex >= _narrationAudioEvents.Length)
        {
            Debug.LogError("Clip index out of range.");
            return;
        }

            
        StartCoroutine(PlayNarrationClipCoroutine(clipIndex, onAudioEndCallback));
    }

    public void PlayBGM()
    {
        _bgmSource.PlayOneShot(_bgMusic, .25f);
    }
    
    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    // private void PlayClipImmediately(int clipIndex)
    // {
    //     if (_onboardSource.isPlaying)
    //     {
    //         _onboardSource.Stop();
    //     }
    //
    //     _onboardSource.PlayOneShot(_onboardClips[clipIndex]);
    //     
    // }
    
    /// <summary>
    /// The following Coroutine is a very general delay options.  I would like to rewrite to allow more precise control of audio delays.
    /// </summary>
    /// <param name="clipIndex"></param>
    /// <returns></returns>
    private IEnumerator PlayNarrationClipCoroutine(int clipIndex, Action onAudioEndCallback = null)
    {
        _audioSkipped = true;
        
        var narrationEvent = _narrationAudioEvents[clipIndex];
        
        yield return new WaitForSeconds(narrationEvent.preDelay);  // Wait for 0 seconds.  Can be changed later if delays are needed.

        if (_onboardSource.isPlaying)
        {
            _onboardSource.Stop();
        }

        yield return null;
        
        _onboardSource.PlayOneShot(narrationEvent.audioClip);
        _audioSkipped = false;

        while (_onboardSource.isPlaying)
        {
            if (_audioSkipped)  // skip remaining logic if replaced with another audio event call
                yield break;

            yield return null;
        }
        
        yield return new WaitUntil(() => !_onboardSource.isPlaying);
        yield return new WaitForSeconds(narrationEvent.postDelay);

        onAudioEndCallback?.Invoke();
    }

    

}