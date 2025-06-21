using System;
using System.Collections.Generic;
using AREditor.LoadObject;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.UI;

public class ARMarkerSpawner : MonoBehaviour
{
    [SerializeField] private Image _markerImage;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private ARRaycastManager raycastManager;

    public static Action<Vector3, Quaternion, string, string> OnPositionDebug; 
    
    public bool isSpawning = false;

    private void OnEnable()
    {
        TouchInputManager.OnTouchPerformed += HandleTouch;
    }

    private void OnDisable()
    {
        TouchInputManager.OnTouchPerformed -= HandleTouch;
    }

    public void OnSpawnButtonPressed()
    {
        isSpawning = true;
        _markerImage.gameObject.SetActive(true);;
    }

    public void OffSpawnButtonPressed()
    {
        isSpawning = false;
        _markerImage.gameObject.SetActive(false);
    }


    private void HandleTouch(Vector2 screenPos)
    {
        if (!isSpawning)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        List<ARRaycastHit> hits = new();
        if (raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            GameObject marker = Instantiate(markerPrefab, hitPose.position, hitPose.rotation);

            MarkerData data = new MarkerData(
                Guid.NewGuid().ToString(),
                10001,
                1,
                0,
                0,
                hitPose.position,
                hitPose.rotation,
                MarkerSpawnType.Base,
                MarkerType.DropItem
            );

            var newMarker = marker.GetComponent<MarkerDataComponent>();
            if (newMarker != null)
            {
                newMarker.markerData = data;
            }
        }
    }
}
    
    
