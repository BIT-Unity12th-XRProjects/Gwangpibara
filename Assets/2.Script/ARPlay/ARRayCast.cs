using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ARRayCast : MonoBehaviour
{
    [SerializeField] private Camera _arCamera; // AR 카메라 (Main Camera 연결)
    [SerializeField] private float _rayDistance = 1.0f; // 레이 길이

    [Header("OverlapSpere")]
    [SerializeField] private float _viewRadius = 1f;
    [SerializeField, Range(0, 360)] private float viewAngle = 20f;
    [SerializeField] private LayerMask _arObjectLayer;
    [SerializeField] private LayerMask _wallLayer;

    private PlayerInputActions _inputActions;
    private HashSet<GameObject> _currentlyDetected = new HashSet<GameObject>();
    private HashSet<GameObject> _previouslyDetected = new HashSet<GameObject>();

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
        CheckCloseOverlap();
    }

    private void CheckCloseOverlap()
    {
        if (_arCamera == null) return;

        Vector3 origin = _arCamera.transform.position;
        Vector3 forward = _arCamera.transform.forward;

        _currentlyDetected.Clear();
        Collider[] hits = Physics.OverlapSphere(origin, _viewRadius, _arObjectLayer);

        foreach (Collider hit in hits)
        {
            GameObject targetObj = hit.gameObject;
            Vector3 dirToTarget = (targetObj.transform.position - origin).normalized;
            float angle = Vector3.Angle(forward, dirToTarget);

            if (angle < viewAngle * 0.5f)
            {
                float distance = Vector3.Distance(origin, targetObj.transform.position);

                if (Physics.Raycast(origin, dirToTarget, out RaycastHit rayHit, distance, _arObjectLayer | _wallLayer))
                {
                    if (rayHit.collider.gameObject.layer == 0)
                    {
                        if (!_previouslyDetected.Contains(targetObj))
                        {
                            targetObj.GetComponent<IDetect>()?.TakeCloseOverlap();
                        }
                        _currentlyDetected.Add(targetObj);
                    }
                }
            }
        }

        foreach (var prev in _previouslyDetected)
        {
            if (!_currentlyDetected.Contains(prev))
            {
                prev.GetComponent<IDetect>()?.NotTakeDetect();
            }
        }

        _previouslyDetected.Clear();
        foreach (var obj in _currentlyDetected)
        {
            _previouslyDetected.Add(obj);
        }
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

                IDetect ARObject = hitObject.GetComponent<IDetect>();

                if (ARObject != null)
                {
                    ARObject.TakeRayHit();
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
        if (Touchscreen.current == null) return;

        Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Ray ray = _arCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (Vector3.Distance(hit.point, _arCamera.transform.position) <= _rayDistance)
            {
                IDetect ARObject = hit.collider.gameObject.GetComponent<IDetect>();
                if (ARObject != null)
                {
                    ARObject.TakeClick();
                }
            }
        }
    }
}
