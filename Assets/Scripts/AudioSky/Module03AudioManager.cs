using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module03AudioManager : MonoBehaviour
{
    public static Module03AudioManager instance;

    [Header("Module Components")]
    [SerializeField] private Module3 module3Manager;
   
    [Header("VOICEOVER")]
    // Dedicated AudioSource for voiceover
    [SerializeField] private AudioSource _sourceModule01;
    [SerializeField] AudioClip[] _clipsModule01;  // Audio clips for Module One Voiceover
    [SerializeField] private NarrationAudioEvent[] _narrationAudioEvents;
    
    private bool _audioSkipped = true;

    private void Start()
    {
        module3Manager.OnIntroEvent.AddListener(() =>
        {
            // happens when wall is cracking
            
            int index = 0; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module3Manager.OnTransitionedToVREvent.AddListener(() =>
        {
            // happens when user teleports outside MRUK room
            
            int index = 0; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module3Manager.OnTaskStartEvent.AddListener(() =>
        {
            // happens when user teleports near mixer table
            
            int index = 0; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module3Manager.OnTaskFailedEvent.AddListener(() =>
        {
            // happens when user fails to connect all required connections properly
            
            int index = 0; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module3Manager.OnTaskFinishedEvent.AddListener(() =>
        {
            // happens when user successfully connects all required connections, and about to start performance
            
            int index = 0; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module3Manager.OnPerformanceStartEvent.AddListener(() =>
        {
            // happens when performance starts
            
            int index = 0; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
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
