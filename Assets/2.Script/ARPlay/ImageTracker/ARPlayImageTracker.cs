using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlayImageTracker : MonoBehaviour
{
    private ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] private GameObject _imagePrefab;
    
    private Dictionary<TrackableId, GameObject> _placeMarkers = new Dictionary<TrackableId, GameObject>();
    
    public delegate void TrackingStartedHandler(ARTrackedImage image, GameObject prefab);
    public event TrackingStartedHandler OnTrackingStarted;    
    
    private Dictionary<TrackableId, int> _trackingCounts = new();
    
    private void Awake()
    {
        if (_arTrackedImageManager == null)
        {
            _arTrackedImageManager = FindAnyObjectByType<ARTrackedImageManager>();
        }
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
                _placeMarkers.Add(image.trackableId, Instantiate(_imagePrefab, image.pose.position, image.pose.rotation));
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
            
            if (_trackingCounts[image.trackableId] < 3)
            {
                _trackingCounts[image.trackableId]++;
            }
            
            if (image.referenceImage.name == "ARPlayImage" && _trackingCounts[image.trackableId] == 3)
            {
                
                OnTrackingStarted?.Invoke(image, _placeMarkers[image.trackableId]);
            }

            
        }

        foreach (KeyValuePair<TrackableId, ARTrackedImage> image in changedArgs.removed)
        {
            _placeMarkers.Remove(image.Key);
        }
    }
    
    

}
