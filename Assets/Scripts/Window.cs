using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour, IModifyInsanity
{
    private bool _isOpen;

    [SerializeField] private float increaseTimerInSeconds;
    [SerializeField] private List<AudioClip> _tripFriendOpenAudioClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _tripFriendClosedAudioClips = new List<AudioClip>();

    private float _timer;

    [SerializeField] private Transform _shutterTransform;
    [SerializeField] private float _windowGracePeriod = 10f;

    [SerializeField] private Vector3 _openPos;
    [SerializeField] private Vector3 _closedPos;
    [SerializeField] private float _moveSpeed = 0.3f;

    private bool _opening = false;
    private bool _closing = false;

    private float _openPercentage = 0f;
    private float _margin = 0.2f;

    private float _currentInsanityModifier = 0f;
    private float _deltaInsanity = 0.01f;
    private bool _shouldIncreaseModifier = true;

    void Start()
    {
        TripFriend.Instance.RegisterInsanityModifier(this);
    }

    private void OnDestroy()
    {
        TripFriend.Instance.UnregisterInsanityModifier(this);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _windowGracePeriod)
        {
            if (_shouldIncreaseModifier)
            {
                StartCoroutine(IncreaseMultiplier());
            }
        }

        if (_opening)
        {
            _openPercentage += _moveSpeed * Time.deltaTime;
        }

        if (_closing)
        {
            _openPercentage -= _moveSpeed * Time.deltaTime;
        }

        _openPercentage = Mathf.Clamp01(_openPercentage);

        _shutterTransform.localPosition = Vector3.Lerp(_closedPos, _openPos, _openPercentage);

        if (_openPercentage > 1f - _margin)
        {
            if (!_isOpen)
            {
                _isOpen = true;
                ResetInsanityModifier();
            }
        }
        else if (_openPercentage < _margin)
        {
            if (_isOpen)
            {
                _isOpen = false;
                ResetInsanityModifier();
            }
        }

    }
    public void ResetInsanityModifier()
    {
        _currentInsanityModifier = 0f;
        _timer = 0f;
    }

    public void SetOpening(bool open)
    {
        _opening = open;
    }

    public void SetClosing(bool closing)
    {
        _closing = closing;
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

    public bool GetIsOpen()
    {
        return _isOpen;
    }

    public AudioClip GetRandomOpenVoiceClip()
    {
        if (_tripFriendOpenAudioClips.Count == 0)
        {
            return null;
        }
        return _tripFriendOpenAudioClips[Random.Range(0, _tripFriendOpenAudioClips.Count)];
    }

    public AudioClip GetRandomClosedVoiceClip()
    {
        if (_tripFriendClosedAudioClips.Count == 0)
        {
            return null;
        }
        return _tripFriendClosedAudioClips[Random.Range(0, _tripFriendClosedAudioClips.Count)];
    }

    private IEnumerator IncreaseMultiplier()
    {
        _shouldIncreaseModifier = false;
        _currentInsanityModifier += _deltaInsanity;
        yield return new WaitForSeconds(increaseTimerInSeconds);
        _shouldIncreaseModifier = true;
    }

    public EInsanityModifierType GetModifierType()
    {
        return EInsanityModifierType.Window;
    }
}
