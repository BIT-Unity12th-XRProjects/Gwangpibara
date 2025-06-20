using System;
using System.Collections.Generic;
using AREditor.DeleteObject;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

public class SearchPosition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _imagePositionText;
    [SerializeField] private TextMeshProUGUI _objectPositionText;
    [SerializeField] private TextMeshProUGUI _debugObjectPositionText;
    
    [SerializeField] private Camera arCamera;

    [SerializeField] private InputActionReference tapPosition;
    [SerializeField] private InputActionReference tapPress;
    
    [SerializeField] private UpdateMarkerDataUI _updateMarkerDataUI;
    [SerializeField] private ARMarkerSpawner _arMarkerSpawner;
    [SerializeField] private MarkerEraser _markerEraser;
    [SerializeField] private MarkerMover _markerMover;
    
    
    private Transform _trackedImageTransform;
    public Transform GetTrackedImageTransform() => _trackedImageTransform;

    private string imageName;
    private bool isGameStart;
    
    private GameObject _selectedObject;
    private GameObject _previousSelectedObject;
    private Material _previousSelectedMaterial;
    private Color _originalColor;
    private Color _selectedColor = Color.green;
    
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
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
                
                SetSelectedObject(_selectedObject);
                
                if (_markerEraser.isDeleteMode == true)
                {
                    return;
                }

                if (_markerMover.isMoveMode)
                {
                    _markerMover.ShowArrowUI(_selectedObject);
                    return;
                }

                if (_arMarkerSpawner.isSpawning)
                {
                    return;
                }
                _updateMarkerDataUI.Open(_selectedObject);
            }
        }
    }

    private void SetSelectedObject(GameObject newSelected)
    {
        if (_previousSelectedObject != null)
        {
            var renderer = _previousSelectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = _originalColor;
            }
        }
        _previousSelectedObject = newSelected;
    
        var newRenderer = newSelected.GetComponent<Renderer>();
        if (newRenderer != null)
        {
            _originalColor = newRenderer.material.color;
            newRenderer.material.color = _selectedColor;
        }

        _selectedObject = newSelected;
    }
    
    public void EnableMoveMode()
    {
        _markerMover.OnMoveModeButtonPressed();

        if (_selectedObject != null)
        {
            _markerMover.SetSelectedMarker(_selectedObject);
        }
    }
    
}
