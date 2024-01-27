using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MicrowaveDoor : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private Transform _doorTransform = null;
    [SerializeField] private float _rotationSpeed = 10f;

    private bool _isDraggingDoor = false;

    [SerializeField, Range(0, 180f)] private float _maxDoorAngle = 100f;

    private float _currentAngle = 0f;

    private Vector2 _previousMousePosition;

    void Start()
    {
        _interactable.OnInteractableClicked += DoorMoveStart;
        _interactable.OnInteractableReleased += DoorMoveEnd;
    }

    private void OnDestroy()
    {
        _interactable.OnInteractableClicked -= DoorMoveStart;
        _interactable.OnInteractableReleased -= DoorMoveEnd;
    }

    private void DoorMoveStart()
    {
        _isDraggingDoor = true;
    }

    private void DoorMoveEnd()
    {
        _isDraggingDoor = false;
    }

    void Update()
    {
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 deltaMousePos = currentMousePos - _previousMousePosition;

        if (_isDraggingDoor)
        {
            float angleChange = _rotationSpeed * (-deltaMousePos.x / Screen.width);
            _currentAngle += angleChange;
            _currentAngle = Mathf.Clamp(_currentAngle, 0, _maxDoorAngle);

            Debug.Log(_currentAngle);
            _doorTransform.localRotation = Quaternion.Euler(0, _currentAngle, 0);
        }

        _previousMousePosition = currentMousePos;
    }
}
