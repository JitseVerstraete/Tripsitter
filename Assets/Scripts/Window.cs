using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour, IModifyInsanity
{
    private bool _isOpen;

    [SerializeField] private float increaseTimerInSeconds;
    [SerializeField] private List<AudioClip> _tripFriendOpenAudioClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _tripFriendClosedAudioClips = new List<AudioClip>();

    private float _currentInsanityModifier = 0f;
    private float _deltaInsanity = 0.01f;
    private bool _shouldIncreaseModifier = true;


    private void Update()
    {
        if (_shouldIncreaseModifier)
        {
            StartCoroutine(IncreaseMultiplier());
        }
        Debug.Log("___" + _currentInsanityModifier);
    }

    public AudioClip GetAudioClip()
    {   
        AudioClip clip = null;
        clip = _isOpen ? GetRandomOpenVoiceClip() : GetRandomClosedVoiceClip();

        return clip;
    }

    public float GetInsanityModifier()
    {
        return _currentInsanityModifier;
    }

    public void ResetInsanityModifier()
    {
        _currentInsanityModifier = 0f;
    }

    public bool GetIsOpen()
    {
        return _isOpen;
    }

    public AudioClip GetRandomOpenVoiceClip()
    {
        return _tripFriendOpenAudioClips[Random.Range(0, _tripFriendOpenAudioClips.Count)];
    }

    public AudioClip GetRandomClosedVoiceClip()
    {
        return _tripFriendClosedAudioClips[Random.Range(0, _tripFriendClosedAudioClips.Count)];
    }

    private IEnumerator IncreaseMultiplier()
    {
        _shouldIncreaseModifier = false;
        _currentInsanityModifier += _deltaInsanity;
        yield return new WaitForSeconds(increaseTimerInSeconds);
        _shouldIncreaseModifier = true;
    }
}
