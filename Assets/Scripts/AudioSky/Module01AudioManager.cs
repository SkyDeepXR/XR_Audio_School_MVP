using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Module01AudioManager : MonoBehaviour
{
    public static Module01AudioManager instance;

    [Header("Module Components")] 
    [SerializeField] private SubModule module_1a;
    [SerializeField] private Module1A_Manager module1a_Manager;
    [SerializeField] private SubModule module_1b;
    [SerializeField] private QuizManager module_1b_QuizManager;
    [SerializeField] private SubModule module_1c;
   
    [Header("VOICEOVER")]
    // Dedicated AudioSource for voiceover
    [SerializeField] private AudioSource _sourceModule01;
    [SerializeField] AudioClip[] _clipsModule01;  // Audio clips for Module One Voiceover
    [SerializeField] private NarrationAudioEvent[] _narrationAudioEvents;
    
    private bool _audioSkipped = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    private void Start()
    {
        module_1a.OnModuleIntro.AddListener(() =>
        {
            int index = 10; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module_1a.OnModuleStart.AddListener(() =>
        {
            int index = 9; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        
        // TODO edit narration event for all Module1A_Cable list objects in Module1A_Manager.cs
        
        module_1b.OnModuleIntro.AddListener(() =>
        {
            int index = 0; // TODO replace with corresponding index, auto starts quiz after VO complete
            StartCoroutine(PlayNarrationClipCoroutine(index, module_1b.StartModule));
        });
        module_1b.OnModuleStart.AddListener(() =>
        {
            int index = 5; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module_1b_QuizManager.OnResultSuccess.AddListener(() =>
        {
            int index = 3; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });
        module_1b_QuizManager.OnResultFailure.AddListener(() =>
        {
            int index = 4; // TODO replace with corresponding index
            StartCoroutine(PlayNarrationClipCoroutine(index));
        });

        module_1c.OnModuleIntro.AddListener(() =>   // happens right after 
        {
            int index = 1; // TODO replace with corresponding index, auto spawns 1st equipment after VO complete
            StartCoroutine(PlayNarrationClipCoroutine(index, module_1c.StartModule));
        });
        
        // TODO go to EquipmentSpawner.cs component and edit narration audio events for each equipment accordingly
        
        module_1c.OnModuleEnd.AddListener(() => // when all equipment is placed
        {
            int index = 2; // TODO replace with corresponding index
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
