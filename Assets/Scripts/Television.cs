using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public enum EChannelType
{
    Good,
    Bad,
}

public class Television : MonoBehaviour, IModifyInsanity
{
    private float _currentInsanityModifier = 0f;
    private EChannelType _currentChannelType;
    private bool _isOn = false;

    [SerializeField] private VideoPlayer _vidPlayer;

    [SerializeField] private List<VideoClip> _goodChannels = new List<VideoClip>();
    [SerializeField] private List<VideoClip> _badChannels = new List<VideoClip>();
    [SerializeField] private List<AudioClip> _tripFriendPositiveAudioClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _tripFriendNegativeAudioClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _tripFriendOffAudioClips = new List<AudioClip>();

    [Space(5)]
    [SerializeField, Range(-1f, 0f)] private float _goodChannelInsanityModifier = -0.5f;
    [SerializeField, Range(0f, 2f)] private float _badChannelInsanityModifier = 0.75f;

    void Start()
    {
        TripFriend.Instance.RegisterInsanityModifier(this);
    }

    private void OnDestroy()
    {
        TripFriend.Instance.UnregisterInsanityModifier(this);
        _vidPlayer.targetTexture.Release();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchToChannel(Random.Range(0, 2) == 0 ? EChannelType.Good : EChannelType.Bad);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleTv(!_isOn);
        }
    }

    public float GetInsanityModifier()
    {
        return _currentInsanityModifier;
    }

    public void SwitchToChannel(EChannelType type)
    {
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
            default:
                break;
        }

        _currentChannelType = type;
        _vidPlayer.clip = newClip;
    }

    public void ToggleTv(bool on)
    {
        _isOn = on;
        if (on)
        {
            SwitchToChannel(Random.Range(0, 2) == 0 ? EChannelType.Good : EChannelType.Bad);
            _vidPlayer.Play();
        }
        else
        {
            _vidPlayer.clip = null;
            _vidPlayer.Stop();
            _vidPlayer.targetTexture.Release();
            _currentInsanityModifier = 0f;
        }
    }

    public VideoClip GetRandomGoodChannel()
    {
        return _goodChannels[Random.Range(0, _goodChannels.Count)];
    }

    public AudioClip GetRandomGoodVoiceClip()
    {
        return _tripFriendPositiveAudioClips[Random.Range(0, _tripFriendPositiveAudioClips.Count)];
    }

    public VideoClip GetRandomBadChannel()
    {
        return _badChannels[Random.Range(0, _badChannels.Count)];
    }

    public AudioClip GetRandomBadVoiceClip()
    {
        return _tripFriendNegativeAudioClips[Random.Range(0, _tripFriendNegativeAudioClips.Count)];
    }

    public AudioClip GetRandomOffVoiceClip()
    {
        return _tripFriendOffAudioClips[Random.Range(0, _tripFriendOffAudioClips.Count)];
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
                    audioclip = GetRandomBadVoiceClip();
                    _currentInsanityModifier = _badChannelInsanityModifier;
                    break;
                default:
                    break;
            }
        } else
        {
            audioclip = GetRandomOffVoiceClip();
        }
        return audioclip;
    }
}
