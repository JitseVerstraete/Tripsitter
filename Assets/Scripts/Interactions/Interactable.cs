using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    public delegate void InteractableEvent();
    public InteractableEvent OnInteractableClicked;
    public InteractableEvent OnInteractableReleased;

    [SerializeField] private MeshRenderer _meshRen;

    public Collider Collider { get { return _col; } }
    private Collider _col;

    private bool _clickDown = false;

    private void Awake()
    {
        _col = GetComponent<Collider>();
        if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
        {
            Debug.LogError("set the interactable onbject on the interactable layer!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RaycastHelper.Instance.InteractableHit == this)
            {
                OnInteractableClicked?.Invoke();
                _clickDown = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_clickDown)
            {
                OnInteractableReleased?.Invoke();
                _clickDown = false;
            }
        }
    }

    public void SetHover(bool hover)
    {
        if (_meshRen == null)
        {
            Debug.LogError($"{gameObject} meshren no assign");
        }
        Material mymat = _meshRen.material;
        if (hover)
        {
            mymat.EnableKeyword("_EMISSION");
            mymat.SetColor("_EmissionColor", new Color(1f, 1f, 1f, 0.2f));
        }
        else
        {
            mymat.DisableKeyword("_EMISSION");
        }
    }

}
