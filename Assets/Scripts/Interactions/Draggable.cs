using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    private Vector3 _colliderBounds = Vector3.zero;

    [SerializeField] float _offsetDistance = 0.5f;

    private bool _isDragging = false;

    private void Awake()
    {
        _interactable.OnInteractableClicked += StartDrag;
        _interactable.OnInteractableReleased += EndDrag;
    }


    private void StartDrag()
    {
        _isDragging = true;
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector3 cameraDir = (Camera.main.transform.position - RaycastHelper.Instance.HitInfo.point).normalized;

            transform.position = RaycastHelper.Instance.HitInfo.point + (cameraDir * _offsetDistance);
        }
    }

    private void EndDrag()
    {
        _isDragging = false;
    }

}
