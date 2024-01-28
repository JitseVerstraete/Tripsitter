using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Video;

public enum EChannelType
{
    Good,
    Bad,
    Static,
}

public class Television : MonoBehaviour, IModifyInsanity
{
    private float _currentInsanityModifier = 0f;
    private EChannelType _currentChannelType;
    private bool _isOn = false;

    private float _goodChannelAngle = 0f;
    private float _badChannelAngle = 0f;
    private int _goodChannelIndex = 0;
    private int _badChannelIndex = 0;


    [SerializeField] private VideoPlayer _vidPlayer;
    [SerializeField] private AudioSource _vidAudioSource;

    [SerializeField] private RotationButton _channelRotation;
    [SerializeField] private float _angleMargin = 20f;

    [SerializeField] private float _timeBetweenSwitchEvents = 10f;
    private float _timer = 0f;

    [SerializeField] private List<VideoClip> _goodChannels = new List<VideoClip>();
    [SerializeField] private List<VideoClip> _badChannels = new List<VideoClip>();
    [SerializeField] private VideoClip _staticClip;
    [SerializeField] private List<AudioClip> _tripFriendPositiveAudioClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _tripFriendNegativeAudioClips = new List<AudioClip>();

    [Space(5)]
    [SerializeField, Range(-1f, 0f)] private float _goodChannelInsanityModifier = -0.5f;
    [SerializeField, Range(0f, 2f)] private float _badChannelInsanityModifier = 0.75f;
    [SerializeField, Range(0f, 2f)] private float _staticChannelInanityModifier = 0.15f;


    void Start()
    {
        TripFriend.Instance.RegisterInsanityModifier(this);
        GenerateNewChannelAngles();
        SetTvOn(false);

        _vidPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _vidPlayer.SetTargetAudioSource(0, _vidAudioSource);
    }

    private void OnDestroy()
    {
        TripFriend.Instance.UnregisterInsanityModifier(this);
        _vidPlayer.targetTexture.Release();
    }

    void Update()
    {
        if (!_isOn || _currentChannelType == EChannelType.Good)
        {
            _timer += Time.deltaTime;
        }

        if (_timer > _timeBetweenSwitchEvents)
        {
            _timer = 0f;
            TvSwitchEvent();
        }

        if (_isOn)
        {
            if (Mathf.Abs(_channelRotation.CurrentAngle - _badChannelAngle) < _angleMargin)
            {
                SwitchToChannel(EChannelType.Bad);
            }
            else if (Mathf.Abs(_channelRotation.CurrentAngle - _goodChannelAngle) < _angleMargin)
            {
                SwitchToChannel(EChannelType.Good);
            }
            else
            {
                SwitchToChannel(EChannelType.Static);
            }
        }
    }

    public float GetInsanityModifier()
    {
        return _currentInsanityModifier;
    }

    public void SwitchToChannel(EChannelType type)
    {
        if (_currentChannelType == type)
        {
            return;
        }

        VideoClip newClip = null;
        switch (type)
        {
            case EChannelType.Good:
                newClip = GetRandomGoodChannel();
                _currentInsanityModifier = _goodChannelInsanityModifier;
                break;
            case EChannelType.Bad:
                newClip = GetRandomBadChannel();
                _currentInsanityModifier = _badChannelInsanityModifier;
                break;
            case EChannelType.Static:
                newClip = _staticClip;
                _currentInsanityModifier = _staticChannelInanityModifier;
                break;
            default:
                break;
        }

        _currentChannelType = type;
        _vidPlayer.clip = newClip;
        _vidPlayer.time = UnityEngine.Random.Range(0, (float)newClip.length / 2f);
    }

    public void SetTvOn(bool on)
    {
        _isOn = on;
        if (on)
        {
            SwitchToChannel(EChannelType.Bad);
            _vidPlayer.Play();
        }
        else
        {
            _vidPlayer.Stop();
            _vidPlayer.targetTexture.Release();
            _currentInsanityModifier = 0f;
        }
    }

    public void ToggleTv()
    {
        SetTvOn(!_isOn);
    }

    public VideoClip GetRandomGoodChannel()
    {
        return _goodChannels[_goodChannelIndex];
    }

    public AudioClip GetRandomGoodVoiceClip()
    {
        if (_tripFriendPositiveAudioClips.Count == 0)
        {
            return null;
        }
        return _tripFriendPositiveAudioClips[Random.Range(0, _tripFriendPositiveAudioClips.Count)];
    }

    public VideoClip GetRandomBadChannel()
    {
        return _badChannels[_badChannelIndex];
    }

    public AudioClip GetRandomBadVoiceClip()
    {
        if (_tripFriendNegativeAudioClips.Count == 0)
        {
            return null;
        }
        return _tripFriendNegativeAudioClips[Random.Range(0, _tripFriendNegativeAudioClips.Count)];
    }

    public AudioClip GetAudioClip()
    {
        AudioClip audioclip = null;
        if (_isOn)
        {
            switch (_currentChannelType)
            {
                case EChannelType.Good:
                    audioclip = GetRandomGoodVoiceClip();
                    _currentInsanityModifier = _goodChannelInsanityModifier;
                    break;
                case EChannelType.Bad:
                case EChannelType.Static:
                    audioclip = GetRandomBadVoiceClip();
                    _currentInsanityModifier = _badChannelInsanityModifier;
                    break;
                default:
                    break;
            }
        }
        else
        {
            audioclip = null;
        }
        return audioclip;
    }


    private void TvSwitchEvent()
    {
        if (!_isOn)
        {
            SetTvOn(true);
        }

        if (_currentChannelType == EChannelType.Bad)
        {
            return;
        }

        SwitchToChannel(EChannelType.Bad);
        GenerateNewChannelAngles();
    }

    private void GenerateNewChannelAngles()
    {
        _badChannelAngle = _channelRotation.CurrentAngle;
        _goodChannelAngle = _channelRotation.CurrentAngle + Random.Range(_angleMargin * 2f, 360f - (_angleMargin * 4f));

        _goodChannelIndex = Random.Range(0, _goodChannels.Count);
        _badChannelIndex = Random.Range(0, _badChannels.Count);

        while (_goodChannelAngle > 360f)
        {
            _goodChannelAngle -= 360f;
        }

        while (_goodChannelAngle < 0f)
        {
            _goodChannelAngle += 360f;
        }
    }

    public EInsanityModifierType GetModifierType()
    {
        return EInsanityModifierType.TV;
    }
}
