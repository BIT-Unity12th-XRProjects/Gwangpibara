using UnityEngine;
using UnityEngine.InputSystem;

public class ARRayCast : MonoBehaviour
{
    [SerializeField] private Camera _arCamera; // AR 카메라 (Main Camera 연결)
    [SerializeField] private float _rayDistance = 1.0f; // 레이 길이

    private PlayerInputActions _inputActions;
    private GameObject _lastCheckObject = null;
    private bool _hasFinding = false;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Click.performed += OnClick;
    }

    private void OnDisable()
    {
        _inputActions.Player.Click.performed -= OnClick;
        _inputActions.Player.Disable();
    }

    void Update()
    {
        CheckRay();
    }

    private void CheckRay()
    {
        Ray ray = new Ray(_arCamera.transform.position, _arCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (_hasFinding == false || _lastCheckObject != hitObject)
            {
                _lastCheckObject = hitObject;

                ARMarkerObject markerObject = hitObject.GetComponent<ARMarkerObject>();

                if (markerObject != null)
                {
                    markerObject.TakeRayHit(); // 한 번만 실행됨
                    _hasFinding = true;
                }
            }
        }
        else
        {
            _lastCheckObject = null;
            _hasFinding = false;
        }
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if(Touchscreen.current == null)
        {
            return;
        }

        Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Ray ray = _arCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ARMarkerObject markerObject = hit.collider.GetComponent<ARMarkerObject>();

            if (markerObject != null)
            {
                markerObject.TakeClick();
            }
        }
    }
}
