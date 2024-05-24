using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module02AudioManager : MonoBehaviour
{
    public static Module02AudioManager instance;
    
   
    [Header("VOICEOVER")]
    // Dedicated AudioSource for voiceover
    [SerializeField] private AudioSource _sourceModule01;
    [SerializeField] AudioClip[] _clipsModule01;  // Audio clips for Module One Voiceover
    
    
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
}
