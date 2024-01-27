using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private float _screenFractionScroll = 0.1f;
    [SerializeField] private float _maxScrollSpeed = 5f;
    [SerializeField] private float _maxScrollAngle = 10f;

    private float _cameraStartAngle = 0;
    private float _currentAngleDiff = 0;

    private void Awake()
    {
        _cameraStartAngle = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        float speedMultiplier = 0f;
        if (mousePos.x > 0f && mousePos.x < (Screen.width * _screenFractionScroll))
        {
            float scrollTreshold = Screen.width * _screenFractionScroll;

            speedMultiplier = -Mathf.Abs(scrollTreshold - mousePos.x) / scrollTreshold;
            _currentAngleDiff += _maxScrollSpeed * Time.deltaTime * speedMultiplier;
        }
        else if (mousePos.x < Screen.width && mousePos.x > Screen.width - (Screen.width * _screenFractionScroll))
        {
            float scrollTreshold = Screen.width - Screen.width * _screenFractionScroll;

            speedMultiplier = Mathf.Abs(mousePos.x - scrollTreshold) / (Screen.width - scrollTreshold);
            _currentAngleDiff += _maxScrollSpeed * Time.deltaTime * speedMultiplier;
        }

        _currentAngleDiff = Mathf.Clamp(_currentAngleDiff, -_maxScrollAngle, _maxScrollAngle);

        transform.rotation = Quaternion.Euler(transform.rotation.x, _cameraStartAngle + _currentAngleDiff, transform.rotation.z);
    }
}
