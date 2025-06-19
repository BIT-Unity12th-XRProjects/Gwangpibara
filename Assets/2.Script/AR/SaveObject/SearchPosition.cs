using System;
using System.Collections.Generic;
using AREditor.DeleteObject;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

public class SearchPosition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _imagePositionText;
    [SerializeField] private TextMeshProUGUI _objectPositionText;
    [SerializeField] private TextMeshProUGUI _debugObjectPositionText;
    
    [SerializeField] private Camera arCamera;

    [SerializeField] private InputActionReference tapPosition;
    [SerializeField] private InputActionReference tapPress;
    
    [SerializeField] private UpdateMarkerDataUI updateMarkerDataUI;
    [SerializeField] private MarkerEraser _markerEraser;
    
    
    private Transform _trackedImageTransform;
    public Transform GetTrackedImageTransform() => _trackedImageTransform;

    private string imageName;
    private bool isGameStart;
    
    private GameObject _selectedObject;

    private void Start()
    {
        isGameStart = false;
    }

    private void OnEnable()
    {
        TouchInputManager.OnTouchPerformed += CheckPosition;
        
    }

    private void OnDisable()
    {
        TouchInputManager.OnTouchPerformed -= CheckPosition;
    }

    public void SetTrackedImagePosition(Transform trackedImageTransform)
    {
        if (isGameStart) return;

        _trackedImageTransform = trackedImageTransform;
        imageName = trackedImageTransform.name;
        isGameStart = true;

        _imagePositionText.text = "ImagePosition " + _trackedImageTransform.position.ToString();
    }

    private void CheckPosition(Vector2 screenPosition)
    {
        _imagePositionText.text = null;
        _objectPositionText.text = null;
        
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 5f);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("ObjectPosition"))
            {
                _objectPositionText.text = "MarkerPosition" + hit.collider.gameObject.transform.position.ToString();
                _selectedObject = hit.collider.gameObject;
                
                if (_markerEraser.isDeleteMode == true)
                {
                    return;
                }
                
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                updateMarkerDataUI.Open(_selectedObject);
            }
        }
    }

}
