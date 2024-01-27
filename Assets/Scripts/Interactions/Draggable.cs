using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    private Vector3 _colliderBounds = Vector3.zero;

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
            Vector3 size = _interactable.Collider.bounds.size * 2f;
            Vector3 camOffset = new Vector3(cameraDir.x * size.x, cameraDir.y * size.y, cameraDir.z * size.z);
            transform.position = RaycastHelper.Instance.HitInfo.point + camOffset;
        }
    }

    private void EndDrag()
    {
        _isDragging = false;
    }

}
