using UnityEngine;
using UnityEngine.InputSystem;

public class ARRayCast : MonoBehaviour
{
    [SerializeField] private Camera _arCamera; // AR 카메라 (Main Camera 연결)
    [SerializeField] private float _rayDistance = 1.0f; // 레이 길이

    private PlayerInputActions _inputActions;
    private GameObject _lastCheckPrefab = null;
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
            if (_lastCheckPrefab != hit.collider.gameObject) // 마지막 확인된 오브젝트인지 확인
            {
                Debug.Log("Ray hit object: " + hit.collider.gameObject.name);

                // TODO : 오브젝트 상호작용

                ARMarkerObject markerObject = hit.collider.gameObject.GetComponent<ARMarkerObject>();
                if (markerObject != null)
                {
                    markerObject.TakeRayHit();
                    _hasFinding = true;
                }
            }
            else
            {
                if (_hasFinding && _lastCheckPrefab != null)
                {
                    _lastCheckPrefab = null;
                    _hasFinding = false;
                }
            }
        }
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

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
