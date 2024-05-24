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
    
    /// <summary>
    /// The Koreography plugin requires more configuration.  You will see the beginnings of the code in this script. But
    /// I'm actually only using standard methods to trigger audio clips through events patching.
    /// </summary>
    // [EventID]
    // [SerializeField] string[] _eventID;
    //
    // [SerializeField] private KoreographyTrack _koreoClip;

    // Use this for initialization
    void Start()
    {
        //Koreographer.Instance.RegisterForEvents("TestEventID", FireEventDebugLog);
    }
    void FireEventDebugLog(KoreographyEvent koreoEvent)
    {
        Debug.Log("Koreography Event Fired.");
    }

    
    public void PlayNarrationClip(int clipIndex)  // Play narration clip according to their index number. Call this from an Event Manager.
    {
        if (clipIndex < 0 || clipIndex >= _onboardClips.Length)
        {
            Debug.LogError("Clip index out of range.");
            return;
        }

        if (clipIndex == 0)
        {
            PlayClipImmediately(clipIndex);
        }
        else
        {
            StartCoroutine(DelayedPlay(clipIndex));
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
    private IEnumerator DelayedPlay(int clipIndex)
    {
        yield return new WaitForSeconds(0f);  // Wait for 0 seconds.  Can be changed later if delays are needed.

        if (_onboardSource.isPlaying)
        {
            _onboardSource.Stop();
        }

        _onboardSource.PlayOneShot(_onboardClips[clipIndex]);
        
    }

    void PlayKoreographySection()
    {
        //_koreoClip.name.Contains("WelcomeSection");
    }


}
