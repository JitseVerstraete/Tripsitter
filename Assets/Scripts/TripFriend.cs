using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum EInsanityModifierType
{
    TV,
    Window,
    Hunger
}


public interface IModifyInsanity
{
    public abstract float GetInsanityModifier();
    public abstract AudioClip GetAudioClip();
    public abstract EInsanityModifierType GetModifierType();
}


public class TripFriend : MonoBehaviour
{
    public static TripFriend Instance { get { return _instance; } }
    private static TripFriend _instance = null;

    [SerializeField] private Slider _insanitySlider = null;
    [SerializeField] private Animator _animator = null;
    private float _currentInsanity;
    [SerializeField] private float _failureInsanityTreshold = 10f;

    [SerializeField] private float _timeBetweenVoiceLines = 10f;
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private List<AudioClip> _goodFlavorClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _BadFlavorClips = new List<AudioClip>();
    private float _voiceLineTimer = 0f;


    private HashSet<IModifyInsanity> _insanityModifiers = new HashSet<IModifyInsanity>();

    [Tooltip("Gameover menu to trigger when the timer elapsed.")]
    [SerializeField] private GameObject gameOverMenu;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogError("there are multiple trip friends, fix this!");
        }
        _animator.SetFloat("InsanityPercent", 0f);
    }

    private void Update()
    {
        float insanityModTotal = 0;
        foreach (IModifyInsanity insanityMod in _insanityModifiers)
        {
            insanityModTotal += insanityMod.GetInsanityModifier();
        }

        float insanityChange = insanityModTotal * Time.deltaTime;
        _currentInsanity += insanityChange;
        _currentInsanity = Mathf.Max(0, _currentInsanity);

        _animator.SetFloat("InsanityPercent", _currentInsanity / _failureInsanityTreshold);

        UpdateSliderValue();
        if (_currentInsanity >= _failureInsanityTreshold)
        {
            GameOver();
        }

        _voiceLineTimer += Time.deltaTime;
        if (_voiceLineTimer >= _timeBetweenVoiceLines)
        {
            _voiceLineTimer = 0f;
            PlayVoiceLine();
        }

    }

    private void PlayVoiceLine()
    {
        AudioClip newClip = null;
        bool flavorVoice = (UnityEngine.Random.Range(0, 10) % 2 == 0);
        if (flavorVoice)
        {
            newClip = GetFlavorAudioClip();
        }
        else
        {
            IModifyInsanity highestMod = null;
            float highestModifierVal = float.MinValue;
            foreach (IModifyInsanity insanityMod in _insanityModifiers)
            {
                if (insanityMod.GetInsanityModifier() > highestModifierVal)
                {
                    highestMod = insanityMod;
                    highestModifierVal = insanityMod.GetInsanityModifier();
                }
            }

            if (highestModifierVal == float.MinValue)
            {
                newClip = GetFlavorAudioClip();
            }
            else
            {
                newClip = highestMod.GetAudioClip();
            }
        }

        _audioSource.clip = newClip;
    }

    private AudioClip GetFlavorAudioClip()
    {
        if (_currentInsanity / _failureInsanityTreshold > 0.6f)
        {
            return _BadFlavorClips[UnityEngine.Random.Range(0, _BadFlavorClips.Count)];
        }
        else
        {
            return _goodFlavorClips[UnityEngine.Random.Range(0, _goodFlavorClips.Count)];
        }
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
    }

    private void UpdateSliderValue()
    {
        if (_insanitySlider == null)
        {
            return;
        }


        _insanitySlider.value = _currentInsanity / _failureInsanityTreshold;

    }

    public void RegisterInsanityModifier(IModifyInsanity modifier)
    {
        _insanityModifiers.Add(modifier);
    }

    public void UnregisterInsanityModifier(IModifyInsanity modifier)
    {
        _insanityModifiers.Remove(modifier);
    }
}
