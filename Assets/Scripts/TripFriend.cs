using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public interface IModifyInsanity
{
    public abstract float GetInsanityModifier();
    public abstract AudioClip GetAudioClip();
}


public class TripFriend : MonoBehaviour
{
    public static TripFriend Instance { get { return _instance; } }
    private static TripFriend _instance = null;

    [SerializeField] private Slider _insanitySlider = null;
    [SerializeField] private Animator _animator = null;
    private float _currentInsanity;
    [SerializeField] private float _failureInsanityTreshold = 10f;

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
