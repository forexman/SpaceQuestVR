using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ControllerInputListener : MonoBehaviour
{
    public InputActionReference primaryButtonActionReference;
    public UnityEvent onPrimaryButtonPressed;

    private void OnEnable()
    {
        if (primaryButtonActionReference != null)
        {
            primaryButtonActionReference.action.Enable();
            primaryButtonActionReference.action.performed += OnPrimaryButtonPerformed;
        }
    }

    private void OnDisable()
    {
        if (primaryButtonActionReference != null)
        {
            primaryButtonActionReference.action.Disable();
            primaryButtonActionReference.action.performed -= OnPrimaryButtonPerformed;
        }
    }

    private void OnPrimaryButtonPerformed(InputAction.CallbackContext context)
    {
        onPrimaryButtonPressed?.Invoke();
    }
}
