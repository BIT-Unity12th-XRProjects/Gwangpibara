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
    [SerializeField] PositionSampler _positionSampler;
    
    private Dictionary<TrackableId, GameObject> _placeMarkers = new Dictionary<TrackableId, GameObject>();
    public delegate void TrackingStartedHandler(ARTrackedImage image, GameObject prefab);
    public event TrackingStartedHandler OnTrackingStarted;

    private void OnEnable()
    {
        _arTrackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var image in args.added)
        {
            if (!_placeMarkers.ContainsKey(image.trackableId))
            {
                var imageMarker = Instantiate(_placePrefab);
                _placeMarkers.Add(image.trackableId, imageMarker);
                OnTrackingStarted?.Invoke(image, imageMarker);
            }
        }

        foreach (var image in args.updated)
        {
            if (_placeMarkers.TryGetValue(image.trackableId, out var marker))
            {
                if (_positionSampler != null && _positionSampler.IsSampling)
                {
                    marker.transform.position = image.pose.position;
                    marker.transform.rotation = image.pose.rotation;
                }
                
            }
        }
    }

}
