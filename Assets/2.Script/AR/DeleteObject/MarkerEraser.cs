using System;
using AREditor.LoadObject;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

namespace AREditor.DeleteObject
{
    public class MarkerEraser : MonoBehaviour
    {
        [SerializeField] private Camera _arCamera;
        [SerializeField] private SaveMarker saveMarker;
        [SerializeField] private ARMarkerSpawner _arMarkerSpawner;
        [SerializeField] private Image _eraserImage;
        [SerializeField] private Image _markerImage;
    
        public bool isDeleteMode = false;
    
        private void Start()
        {
            isDeleteMode = false;
        }
    
        public void OnclickEraserButton()
        {
            isDeleteMode = true;
            _arMarkerSpawner.isSpawning = false;
            _eraserImage.gameObject.SetActive(true);
            _markerImage.gameObject.SetActive(false);
        }

        public void OffclickEraserButton()
        {
            isDeleteMode = false;
            _eraserImage.gameObject.SetActive(false);
        }


        private void OnEnable()
        {
            TouchInputManager.OnTouchPerformed += TryDeleteMarker;
        }

        private void OnDisable()
        {
            TouchInputManager.OnTouchPerformed -= TryDeleteMarker;
        }

        private void TryDeleteMarker(Vector2 screenPosition)
        {
            if (!isDeleteMode) return;
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = _arCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.CompareTag("ObjectPosition"))
                {
                    var markerDataComponent = hitObj.GetComponentInParent<MarkerDataComponent>();
                    if (markerDataComponent != null)
                    {
                        string markerId = markerDataComponent.markerData.ID;
                        saveMarker.RemoveMarkerData(markerId);
                        Destroy(hitObj);
                        Debug.Log($"오브젝트 {markerId} 삭제됨");
                    }
                }
            }
        }
        
        
    }
}


