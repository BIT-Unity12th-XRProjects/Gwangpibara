using UnityEngine;
using UnityEngine.InputSystem;

public class ItemViewer : MonoBehaviour
{
    private GameObject _itemObject;
    private Camera _cam;
    private GameObject _mapParent;
    [SerializeField] private GameObject _testPrefab;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 0.2f;
    private Vector2 _rotationInput;
    private bool _isDragging = false;
    private PlayerInputActions _inputActions;

    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 0.01f;
    [SerializeField] private float _minZoom = 0.1f;
    [SerializeField] private float _maxZoom = 0.2f;
    private float _prevPinchDistance = 0f;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();

        UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    private void OnEnable()
    {
        _cam = Camera.main;
        _mapParent = GameObject.Find("MapParent");

        _inputActions.Player.Enable();

        _inputActions.Player.Look.performed += OnLookPerformed;
        _inputActions.Player.Look.canceled += CancelLookPerformed;

        _inputActions.Player.Click.started += OnClickStarted;
        _inputActions.Player.Click.canceled += OnClickCanceled;
    }

    private void OnDisable()
    {
        _inputActions.Player.Look.performed -= OnLookPerformed;
        _inputActions.Player.Look.canceled -= CancelLookPerformed;

        _inputActions.Player.Click.started -= OnClickStarted;
        _inputActions.Player.Click.canceled -= OnClickCanceled;

        _inputActions.Player.Disable();
    }


    private void Update()
    {
        RotateObject();
        Zoom();
    }

    public void ViewItem(ItemViewData itemViewData)
    {
        //_name = itemData.Name;
        GameObject targetObject = itemViewData.itemPrefab;

        if (targetObject == null)
        {
            Debug.Log("아이템 데이터가 이상한듯");
            return;
        }

        _mapParent.gameObject.SetActive(false);
        _itemObject = Instantiate(targetObject, new Vector3(0, 0, 0), Quaternion.identity, _cam.transform);

        _itemObject.transform.localScale = new Vector3(_minZoom, _minZoom, _minZoom);
        _itemObject.transform.localPosition = Vector3.forward;
    }

    public void DestroyItem()
    {
        _mapParent.gameObject.SetActive(true);
        Destroy(_itemObject);
    }

    private void Zoom()
    {
        MouseZoom();
        TouchZoom();
    }

    private void MouseZoom()
    {
        // 마우스 휠 줌 처리
        float scrollY = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scrollY) > 0.01f)
        {
            float scaleFactor = 1 + scrollY * _zoomSpeed;
            Vector3 newScale = _itemObject.transform.localScale * scaleFactor;

            // → 추가: 각 축별로 Clamp
            newScale.x = Mathf.Clamp(newScale.x, _minZoom, _maxZoom);
            newScale.y = Mathf.Clamp(newScale.y, _minZoom, _maxZoom);
            newScale.z = Mathf.Clamp(newScale.z, _minZoom, _maxZoom);

            _itemObject.transform.localScale = newScale;
        }
    }
    private void TouchZoom()
    {
        // 터치 핀치 줌 처리
        var touches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
        if (touches.Count >= 2)
        {
            _isDragging = false;

            Vector2 touchPosition1 = touches[0].screenPosition;
            Vector2 touchPosition2 = touches[1].screenPosition;
            float currentDistance = Vector2.Distance(touchPosition1, touchPosition2);

            // 이전값이 0일 땐 movingDistance를 0으로 처리
            float movingDistance;
            if (_prevPinchDistance == 0f)
            {
                movingDistance = 0f;
            }
            else
            {
                movingDistance = currentDistance - _prevPinchDistance;
            }

            float scaleSize = 1 + movingDistance * _zoomSpeed;

            Vector3 newScale = _itemObject.transform.localScale * scaleSize;
            newScale.x = Mathf.Clamp(newScale.x, _minZoom, _maxZoom);
            newScale.y = Mathf.Clamp(newScale.y, _minZoom, _maxZoom);
            newScale.z = Mathf.Clamp(newScale.z, _minZoom, _maxZoom);
            _itemObject.transform.localScale = newScale;

            _prevPinchDistance = currentDistance;
        }
        else
        {
            // 터치가 2개 미만이면 초기화
            _prevPinchDistance = 0f;
        }
    }

    private void RotateObject()
    {
        if (_isDragging && _rotationInput != Vector2.zero)
        {
            float dx = _rotationInput.x;
            float dy = _rotationInput.y;

            // 좌우 회전 
            _itemObject.transform.Rotate(Vector3.up, -dx * _rotationSpeed, Space.World);

            // 상하 회전 
            _itemObject.transform.Rotate(Vector3.right, dy * _rotationSpeed, Space.World);
        }
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        if (_isDragging)
        {
            _rotationInput = context.ReadValue<Vector2>();
        }
    }

    private void CancelLookPerformed(InputAction.CallbackContext context)
    {
        _rotationInput = Vector2.zero;
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        _isDragging = true;
    }

    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        _isDragging = false;
    }
}
