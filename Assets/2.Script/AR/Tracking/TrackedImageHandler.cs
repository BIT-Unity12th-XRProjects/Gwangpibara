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
    [SerializeField] XROrigin _xROrigin;
    [SerializeField] GameObject _placePrefab;
    
    [SerializeField] MarkerLoader _markerLoader;
    [SerializeField] private SearchPosition _searchPosition;
    
    private Dictionary<TrackableId, GameObject> _placeMarkers = new Dictionary<TrackableId, GameObject>();
    
    private bool _isLoadObject = false;
    
    private void Start()
    {
        _arTrackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
        _isLoadObject = false;
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

            if (_isLoadObject == true)
            {
                return;
            }

            _searchPosition.SetTrackedImagePosition(image.transform);
            _markerLoader.LoadAndSpawnMarkers(image.transform);
            _isLoadObject = true;
        }
        
        foreach (ARTrackedImage image in changedArgs.updated)
        {
            if (_placeMarkers.TryGetValue(image.trackableId, out var placeMarker) == false)
            {
                _placeMarkers.Add(image.trackableId, Instantiate(_placePrefab));
            }
            _placeMarkers[image.trackableId].transform.position = image.pose.position;
            _placeMarkers[image.trackableId].transform.rotation = image.pose.rotation;
        }
        

    }

}
