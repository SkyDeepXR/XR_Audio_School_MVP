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
    [SerializeField] private AudioSource _onboardSource;
    [SerializeField] AudioClip[] _onboardClips;  // Audio clips for Onboarding Voiceover
    [SerializeField] private AudioClip[] _pressButtonVO;
    
    
    
    

    // Use this for initialization
    void Start()
    {
        //Koreographer.Instance.RegisterForEvents("TestEventID", FireEventDebugLog);
    }
    void FireEventDebugLog(KoreographyEvent koreoEvent)
    {
        Debug.Log("Koreography Event Fired.");
    }

    
    public void PlayNarrationClip(int clipIndex, float delay)  // Play narration clip according to their index number. Call this from an Event Manager.
    {
        if (clipIndex < 0 || clipIndex >= _onboardClips.Length)
        {
            Debug.LogError("Clip index out of range.");
            return;
        }
        else
        {
            StartCoroutine(DelayedPlay(clipIndex, delay));
        }
    }

    private void PlayClipImmediately(int clipIndex)
    {
        if (_onboardSource.isPlaying)
        {
            _onboardSource.Stop();
        }

        _onboardSource.PlayOneShot(_onboardClips[clipIndex]);
        
    }
    
    /// <summary>
    /// The following Coroutine is a very general delay options.  I would like to rewrite to allow more precise control of audio delays.
    /// </summary>
    /// <param name="clipIndex"></param>
    /// <returns></returns>
    private IEnumerator DelayedPlay(int clipIndex, float delay)
    {
        yield return new WaitForSeconds(delay);  // Wait for 0 seconds.  Can be changed later if delays are needed.

        if (_onboardSource.isPlaying)
        {
            _onboardSource.Stop();
        }

        _onboardSource.PlayOneShot(_onboardClips[clipIndex]);
        
    }

    

}
