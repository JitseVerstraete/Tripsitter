using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Microwave : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private AudioClip _runningSound = null;
    [SerializeField] private AudioClip _readySound = null;
    [SerializeField] private Collider _cookCollider;

    [SerializeField] private MicrowaveDoor _door;
    [SerializeField] private Transform _timerTransform;

    [SerializeField] private float _timeToCook;
    private float _timer = 0;
    private bool _isRunning = false;

    private List<Pizza> _pizzasInside = new List<Pizza>();

    private void Awake()
    {
        _timer = 0;
    }

    public void TurnOn()
    {
        if (!_isRunning && _door.IsClosed)
        {
            _isRunning = true;
            _door.SetLocked(true);

            _audioSource.clip = _runningSound;
            _audioSource.Play();
            Debug.Log("turn on");
        }
    }

    private void Update()
    {
        if (_isRunning)
        {
            _timer += Time.deltaTime;
            _timerTransform.localRotation = Quaternion.Euler(0, 0, 360f * (_timer / _timeToCook));
        }

        if (_timer >= _timeToCook)
        {
            _isRunning = false;
            _timer = 0;
            _timerTransform.localRotation = Quaternion.Euler(0, 0, 0);
            _door.SetLocked(false);

            _audioSource.Stop();
            _audioSource.clip = _readySound;
            _audioSource.Play();

            foreach (Pizza p in _pizzasInside)
            {
                p.CookPizza();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pizza pizza = other.attachedRigidbody.gameObject.GetComponent<Pizza>();
        if (pizza != null)
        {
            _pizzasInside.Add(pizza);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Pizza pizza = other.attachedRigidbody.gameObject.GetComponent<Pizza>();
        if (pizza != null)
        {
            _pizzasInside.Remove(pizza);
        }
    }
}
