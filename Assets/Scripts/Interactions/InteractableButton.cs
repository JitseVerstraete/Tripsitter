using UnityEngine.Events;
using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Interactable _interactable;
    [SerializeField] private UnityEvent _OnPress;

    private const float BUTTON_OFFSET = 0.06f;
    private Vector3 _initialPos = Vector3.zero;


    void Awake()
    {
        _interactable.OnInteractableClicked += PressButton;
        _interactable.OnInteractableReleased += ReleaseButton;
        _initialPos = _transform.localPosition;
    }

    private void OnDestroy()
    {
        _interactable.OnInteractableClicked -= PressButton;
        _interactable.OnInteractableReleased -= ReleaseButton;
    }

    private void PressButton()
    {
        _transform.localPosition = new Vector3(_initialPos.x, _initialPos.y, _initialPos.z - BUTTON_OFFSET);
        _OnPress?.Invoke();
    }

    private void ReleaseButton()
    {
        _transform.localPosition = _initialPos;
    }
}
