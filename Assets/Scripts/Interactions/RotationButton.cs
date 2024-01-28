using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationButton : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private Transform _buttonTransform = null;
    [SerializeField] private float _rotationSpeed = 10f;

    private bool _isRotatingButton = false;

    public float CurrentAngle { get { return _currentAngle; } }
    private float _currentAngle = 0f;

    private Vector2 _previousMousePosition;

    void Start()
    {
        _interactable.OnInteractableClicked += ButtonTurnStart;
        _interactable.OnInteractableReleased += ButtonTurnEnd;
    }

    private void OnDestroy()
    {
        _interactable.OnInteractableClicked -= ButtonTurnStart;
        _interactable.OnInteractableReleased -= ButtonTurnEnd;
    }

    private void ButtonTurnStart()
    {
        _isRotatingButton = true;
    }

    private void ButtonTurnEnd()
    {
        _isRotatingButton = false;
    }

    void Update()
    {
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 deltaMousePos = currentMousePos - _previousMousePosition;

        if (_isRotatingButton)
        {
            float angleChange = _rotationSpeed * (-deltaMousePos.x / Screen.width);
            _currentAngle += angleChange;

            while (_currentAngle > 360f)
            {
                _currentAngle -= 360f;
            }

            while (_currentAngle < 0f)
            {
                _currentAngle += 360f;
            }

            _buttonTransform.localRotation = Quaternion.Euler(0, 0, _currentAngle);
        }

        _previousMousePosition = currentMousePos;
    }

}
