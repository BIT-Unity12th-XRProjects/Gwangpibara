using System;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageHandler : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] GameObject _placePrefab;

    private Dictionary<TrackableId, GameObject> _placeMarkers = new Dictionary<TrackableId, GameObject>();
    public delegate void TrackingStartedHandler(ARTrackedImage image, GameObject prefab);
    public event TrackingStartedHandler OnTrackingStarted;
    
    [SerializeField] private TMP_Text _debugPositionText;
    [SerializeField] SearchPosition _searchPosition;
    [SerializeField] MarkerLoader _markerLoader;
    
    [SerializeField] ARTrackingManager _arTrackingManager;

    private void OnEnable()
    {
        _arTrackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> changedArgs)
    {
        
        foreach (ARTrackedImage image in changedArgs.added)
        {
            if (_placeMarkers.TryGetValue(image.trackableId, out var placeMarker) == false)
            {
                _placeMarkers.Add(image.trackableId, Instantiate(_placePrefab));
            }
            _placeMarkers[image.trackableId].transform.position = image.pose.position;
            _placeMarkers[image.trackableId].transform.rotation = image.pose.rotation;
            OnTrackingStarted?.Invoke(image, _placeMarkers[image.trackableId]);
        }
        
        foreach (ARTrackedImage image in changedArgs.updated)
        {
            if (image.referenceImage.name == "ImageTracker")
            {
                continue;
            }
            
            
            if (_placeMarkers.TryGetValue(image.trackableId, out var placeMarker) == false)
            {
                _placeMarkers.Add(image.trackableId, Instantiate(_placePrefab));
            }
            _placeMarkers[image.trackableId].transform.position = image.pose.position;
            _placeMarkers[image.trackableId].transform.rotation = image.pose.rotation;
        }

        foreach (KeyValuePair<TrackableId, ARTrackedImage> image in changedArgs.removed)
        {
            // 딕셔너리에 있으면 삭제 , 없으면 --
        }

        /*
        foreach (var image in changedArgs.added)
        {
            if (_placeMarkers.TryGetValue(image.trackableId, out var placeMarker) == false)
            {
                var imageMarker = Instantiate(_placePrefab);
                _placeMarkers.Add(image.trackableId, Instantiate(imageMarker));
                OnTrackingStarted?.Invoke(image, imageMarker);
            }

            _placeMarkers[image.trackableId].transform.position = image.pose.position;
            _placeMarkers[image.trackableId].transform.rotation = image.pose.rotation;
        }

        foreach (var image in changedArgs.updated)
        {
            if (_placeMarkers.TryGetValue(image.trackableId, out var placeMarker) == false)
            {
                _placeMarkers.Add(image.trackableId, Instantiate(_placePrefab));
            }
            _placeMarkers[image.trackableId].transform.position = image.pose.position;
            _placeMarkers[image.trackableId].transform.rotation = image.pose.rotation;

        }
        */
    }
}
