using System;
using UnityEngine;

public class MicrowaveDoor : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private Transform _doorTransform = null;
    [SerializeField] private float _rotationSpeed = 10f;

    private bool _isDraggingDoor = false;

    [SerializeField, Range(0, 180f)] private float _maxDoorAngle = 100f;

    public bool IsClosed { get { return _currentAngle <= 0.5f; } }
    private float _currentAngle = 0f;

    private bool _isLocked = false;

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

    public void SetLocked(bool locked)
    {
        _isLocked = locked;
    }

    void Update()
    {
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 deltaMousePos = currentMousePos - _previousMousePosition;

        if (_isDraggingDoor && !_isLocked)
        {
            float angleChange = _rotationSpeed * (-deltaMousePos.x / Screen.width);
            _currentAngle += angleChange;
            _currentAngle = Mathf.Clamp(_currentAngle, 0, _maxDoorAngle);
            _doorTransform.localRotation = Quaternion.Euler(0, _currentAngle, 0);
        }

        _previousMousePosition = currentMousePos;
    }
}
