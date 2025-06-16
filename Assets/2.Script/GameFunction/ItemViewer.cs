using UnityEngine;
using UnityEngine.InputSystem;

public class ItemViewer : MonoBehaviour
{
    [SerializeField] private GameObject _testPrefab;
    [SerializeField] private string _name;
    private bool _canSpawn = true;
    private GameObject _itemObject;


    [SerializeField] private float _rotationSpeed = 0.2f;
    private Vector2 _rotationInput;
    private bool _isDragging = false;
    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
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
        if(_testPrefab != null)
        {
            if(Keyboard.current.gKey.wasPressedThisFrame && _canSpawn)
            {
                _canSpawn = false;
                CheckItemTest(null);
            }

            else if(_canSpawn == false)
            {
                if (_isDragging && _rotationInput != Vector2.zero)
                {
                    _itemObject.transform.Rotate(Vector3.up, -_rotationInput.x * _rotationSpeed, Space.World);
                }
            }
        }
    }

    public void CheckItemTest(ItemData itemData)
    {
        //_name = itemData.Name;
        _itemObject = Instantiate(_testPrefab, new Vector3(0,0,0), Quaternion.identity);
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
