using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module02AudioManager : MonoBehaviour
{
    public static Module02AudioManager instance;

    [Header("Module Components")]
    [SerializeField] private SubModule module2;
    [SerializeField] private Module2 module2Manager;
   
    [Header("VOICEOVER")]
    // Dedicated AudioSource for voiceover
    [SerializeField] private AudioSource _sourceModule01;
    [SerializeField] AudioClip[] _clipsModule01;  // Audio clips for Module One Voiceover
    [SerializeField] private NarrationAudioEvent[] _narrationAudioEvents;
    
    private bool _audioSkipped = true;

    private void Start()
    {
        module2.OnModuleIntro.AddListener(() =>
        {
            // happens when showing lessons UI
            
            int index = 0; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        
        module2Manager.OnTutorial1IntroEvent.AddListener(() =>
        {
            // VO before asking user to connect mic to mixer for the 1st time
            
            int index = 2; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module2Manager.OnTutorial1Event.AddListener(() =>
        {
            // VO when asking user to connect cable to mic
            
            int index = 3; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module2Manager.OnTutorial2IntroEvent.AddListener(() =>
        {
            // VO when user successfully connected cable to mic
            
            int index = 8; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index, module2Manager.GoToNextGameState));
        });
        module2Manager.OnTutorial2Event.AddListener(() =>
        {
            // VO when asking user to connect cable to mixer
            
            int index = 3; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module2Manager.OnTaskIntroEvent.AddListener(() =>
        {
            // VO when user successfully connected cable to mixer and before proceeding to self-connecting task with mic & speakers
            
            int index = 9; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index, module2Manager.GoToNextGameState));
        });
        module2Manager.OnTaskStartEvent.AddListener(() =>
        {
            // VO when user is prompted to connect all equipment based on given diagram
            
            int index = 5; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module2Manager.OnTaskFailedEvent.AddListener(() =>
        {
            // VO when user fails to connect all equipment based on given diagram
            
            int index = 5; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module2Manager.OnTaskFinishedEvent.AddListener(() =>
        {
            // VO when user successfully connects all equipment and completing module 2
            
            int index = 10; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module2Manager.OnTaskRecapEvent.AddListener(() =>
        {
            // VO acknowledging what has been learned.
            
            // int index = 11; // TODO replace with corresponding index
            // StartCoroutine(PlayNarrationClipCoroutine(index));
        });
    }
    
    public void PlayNarrationClip(int clipIndex)  // Play voiceover clip according to their index number. Call this from an Event Manager.
    {
        if (clipIndex < 0 || clipIndex >= _clipsModule01.Length)
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
        if (_sourceModule01.isPlaying)
        {
            _sourceModule01.Stop();
        }

        _sourceModule01.PlayOneShot(_clipsModule01[clipIndex]);
        
    }
    
    /// <summary>
    /// The following Coroutine is a very general delay options.  I would like to rewrite to allow more precise control of audio delays.
    /// </summary>
    /// <param name="clipIndex"></param>
    /// <returns></returns>
    private IEnumerator DelayedPlay(int clipIndex)
    {
        yield return new WaitForSeconds(0f);  // Wait for 0 seconds.  Can be changed later if delays are needed.

        if (_sourceModule01.isPlaying)
        {
            _sourceModule01.Stop();
        }

        _sourceModule01.PlayOneShot(_clipsModule01[clipIndex]);
        
    }
    
    public void StartPlayNarrationClipCoroutine(NarrationAudioEvent narrationEvent, Action onAudioEndCallback = null)
    {
        StartCoroutine(PlayNarrationClipCoroutine(narrationEvent, onAudioEndCallback));
    }
    
    private IEnumerator PlayNarrationClipCoroutine(int clipIndex, Action onAudioEndCallback = null)
    {
        _audioSkipped = true;
        
        var narrationEvent = _narrationAudioEvents[clipIndex];
        
        yield return new WaitForSeconds(narrationEvent.preDelay);  // Wait for 0 seconds.  Can be changed later if delays are needed.

        if (_sourceModule01.isPlaying)
        {
            _sourceModule01.Stop();
        }

        yield return null;
        
        _sourceModule01.PlayOneShot(narrationEvent.audioClip);
        _audioSkipped = false;

        while (_sourceModule01.isPlaying)
        {
            if (_audioSkipped)  // skip remaining logic if replaced with another audio event call
                yield break;

            yield return null;
        }
        
        yield return new WaitUntil(() => !_sourceModule01.isPlaying);
        yield return new WaitForSeconds(narrationEvent.postDelay);

        onAudioEndCallback?.Invoke();
    }
    
    private IEnumerator PlayNarrationClipCoroutine(NarrationAudioEvent narrationEvent, Action onAudioEndCallback = null)
    {
        _audioSkipped = true;
        
        yield return new WaitForSeconds(narrationEvent.preDelay);  // Wait for 0 seconds.  Can be changed later if delays are needed.

        if (_sourceModule01.isPlaying)
        {
            _sourceModule01.Stop();
        }

        yield return null;
        
        _sourceModule01.PlayOneShot(narrationEvent.audioClip);
        _audioSkipped = false;

        while (_sourceModule01.isPlaying)
        {
            if (_audioSkipped)  // skip remaining logic if replaced with another audio event call
                yield break;

            yield return null;
        }
        
        yield return new WaitUntil(() => !_sourceModule01.isPlaying);
        yield return new WaitForSeconds(narrationEvent.postDelay);

        onAudioEndCallback?.Invoke();
    }
}
