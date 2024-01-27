using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    public delegate void InteractableEvent();
    public InteractableEvent OnInteractableClicked;
    public InteractableEvent OnInteractableReleased;

    public Collider Collider { get { return _col; } }
    private Collider _col;

    private bool _clickDown = false;

    private void Awake()
    {
        _col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RaycastHelper.Instance.InteractableHit == this)
            {
                OnInteractableClicked?.Invoke();
                Debug.Log("clicked!");
                _clickDown = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_clickDown)
            {
                OnInteractableReleased?.Invoke();
                Debug.Log("released!");
                _clickDown = false;
            }
        }
    }

}
