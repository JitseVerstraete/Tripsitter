using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RaycastHelper : MonoBehaviour
{
    public static RaycastHelper Instance { get { return _instance; } }
    private static RaycastHelper _instance = null;

    private Camera _camera = null;

    public RaycastHit HitInfo { get { return _hitInfo; } }
    private RaycastHit _hitInfo;

    public Interactable InteractableHit { get { return _hitInteractable; } }
    private Interactable _hitInteractable;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        int interactableMask = LayerMask.GetMask("Interactable");
        Physics.Raycast(ray, out _hitInfo, float.MaxValue, ~interactableMask);

        RaycastHit interactableHit;
        Physics.Raycast(ray, out interactableHit, float.MaxValue, interactableMask);
        if (interactableHit.collider != null)
        {
            _hitInteractable = interactableHit.collider.GetComponent<Interactable>();
        }
        else
        {
            _hitInteractable = null;
        }
    }
}
