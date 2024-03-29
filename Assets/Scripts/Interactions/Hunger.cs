using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour, IModifyInsanity
{
    [SerializeField] private List<AudioClip> _hungerClips;

    private float _currentHungerInsanityMod = 0;
    private float _timer = 0f;
    [SerializeField] private float _hungerGraceTime = 5f;
    [SerializeField] private float _hungerGrowRatePerSecond = 0.01f;

    private void Update()
    {
        TripFriend.Instance.RegisterInsanityModifier(this);

        _timer += Time.deltaTime;

        if (_timer >= _hungerGraceTime)
        {
            _currentHungerInsanityMod += _hungerGrowRatePerSecond * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        TripFriend.Instance.UnregisterInsanityModifier(this);
    }


    private void OnTriggerEnter(Collider other)
    {
        Pizza p = other.attachedRigidbody.GetComponent<Pizza>();
        if (p != null && p.Cooked)
        {
            Destroy(other.attachedRigidbody.gameObject);
            _timer = 0f;
            _currentHungerInsanityMod = 0f;
            TripFriend.Instance.ChangeInsanity(-1f);
        }
    }

    public float GetInsanityModifier()
    {
        return _currentHungerInsanityMod;
    }

    public AudioClip GetAudioClip()
    {
        if (_hungerClips.Count == 0)
        {
            return null;
        }
        return _hungerClips[Random.Range(0, _hungerClips.Count)];
    }

    public EInsanityModifierType GetModifierType()
    {
        return EInsanityModifierType.Hunger;
    }
}
