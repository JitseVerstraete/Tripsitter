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

    [SerializeField] private Image _insanitySlider = null;
    [SerializeField] private Animator _animator = null;
    private float _currentInsanity;
    [SerializeField] private float _failureInsanityTreshold = 10f;

    [SerializeField] private float _timeBetweenVoiceLines = 10f;
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private List<AudioClip> _goodFlavorClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _BadFlavorClips = new List<AudioClip>();
    private float _voiceLineTimer = 0f;

    [SerializeField] private GameObject _pizzaIcon = null;
    [SerializeField] private GameObject _windowIcon = null;
    [SerializeField] private GameObject _tvIcon = null;

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
            switch (insanityMod.GetModifierType())
            {
                case EInsanityModifierType.TV:
                    _tvIcon.SetActive(insanityMod.GetInsanityModifier() > 0.05f);
                    break;
                case EInsanityModifierType.Window:
                    _windowIcon.SetActive(insanityMod.GetInsanityModifier() > 0.05f);
                    break;
                case EInsanityModifierType.Hunger:
                    _pizzaIcon.SetActive(insanityMod.GetInsanityModifier() > 0.05f);
                    break;
            }
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
        bool flavorVoice = (UnityEngine.Random.Range(0f, 1f) > (_currentInsanity / _failureInsanityTreshold) + 0.1f);
        if (flavorVoice)
        {
            newClip = GetFlavorAudioClip();
            Debug.Log("getting flavor clip");
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

            if (highestMod == null || highestMod.GetAudioClip() == null)
            {
                newClip = GetFlavorAudioClip();
                Debug.Log("tried to get relevant audio, but was null, getting flavor");
            }
            else
            {
                newClip = highestMod.GetAudioClip();
                Debug.Log("getting gameplay audio clip");
            }
        }

        _audioSource.clip = newClip;
        _audioSource.Play();
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

        _insanitySlider.fillAmount = _currentInsanity / _failureInsanityTreshold;
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
