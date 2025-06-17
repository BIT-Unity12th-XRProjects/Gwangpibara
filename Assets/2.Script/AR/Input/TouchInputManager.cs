using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchInputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference tapPosition;
    [SerializeField] private InputActionReference tapPress;
    
    public static event Action<Vector2> OnTouchPerformed;

    private bool isPressed;
    private Vector2 cachedTouch;
    
    private void OnEnable()
    {
        tapPosition.action.performed += OnTouchPosition;
        tapPress.action.performed += OnTouchPress;

        tapPosition.action.Enable();
        tapPress.action.Enable();
        
    }

    private void OnDisable()
    {
        tapPosition.action.performed -= OnTouchPosition;
        tapPress.action.performed -= OnTouchPress;

        tapPosition.action.Disable();
        tapPress.action.Disable();
    }
    
    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        cachedTouch = context.ReadValue<Vector2>();
    }

    private void OnTouchPress(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            OnTouchPerformed?.Invoke(cachedTouch);
        }
    }
}
