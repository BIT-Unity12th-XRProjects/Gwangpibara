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
    
    [SerializeField] SearchPosition _searchPosition;
    [SerializeField] MarkerLoader _markerLoader;
    [SerializeField] TMP_Text _debugText;
    
    [SerializeField] ARTrackingManager _arTrackingManager;
    
    Dictionary<TrackableId, int> _trackingCounts = new();

    private bool isSampling;

    private void Awake()
    {
        isSampling = false;
    }

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
                _placeMarkers.Add(image.trackableId, Instantiate(_placePrefab, image.pose.position, image.pose.rotation));
            }

            _placeMarkers[image.trackableId].transform.position = image.pose.position;
            _placeMarkers[image.trackableId].transform.rotation = image.pose.rotation;
            
        }
        
        foreach (ARTrackedImage image in changedArgs.updated)
        {
            if (image.trackingState != TrackingState.Tracking)
            {
                continue;    
            }

            if (_trackingCounts.ContainsKey(image.trackableId) == false)
            {
                _trackingCounts[image.trackableId] = 0;
            }
            
            _trackingCounts[image.trackableId]++;

            if (image.referenceImage.name == "ImageTracker" && _trackingCounts[image.trackableId] == 3)
            {
                _debugText.text = $"[AR] 상태: {image.trackingState}, 이름: {image.referenceImage.name}, 위치: {image.transform.position}";
                if (isSampling)
                {
                    return;
                }
                OnTrackingStarted?.Invoke(image, _placeMarkers[image.trackableId]);
                isSampling = true;
            }
        }

        foreach (KeyValuePair<TrackableId, ARTrackedImage> image in changedArgs.removed)
        {
            _placeMarkers.Remove(image.Key);
        }
    }
}
