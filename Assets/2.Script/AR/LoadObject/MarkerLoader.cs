using System.Collections.Generic;
using System.Linq;
using AREditor.LoadObject;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MarkerLoader : MonoBehaviour
{ 
    [SerializeField] private GameObject markerPrefab;
    private SaveMarkerData _saveMarkerData;
    [SerializeField] private SaveMarker saveMarker;
    
    private bool _isSpawned = false;

    private void Awake()
    {
        _saveMarkerData = new SaveMarkerData();
    }
    
    private List<GameObject> spawnedMarkers = new List<GameObject>();

    
    public void LoadAndSpawnMarkers(Transform imageTransform)
    {
        if (_isSpawned)
        {
            return;
        }
        
        foreach (var obj in spawnedMarkers)
        {
            Destroy(obj);
        }
        spawnedMarkers.Clear();
    
        List<MarkerData> markerDatas = _saveMarkerData.LoadMarkerList();
        
        foreach (var data in markerDatas)
        {
            Vector3 worldPos = imageTransform.TransformPoint(data.position);
            Quaternion worldRot = imageTransform.rotation * data.rotation;
            
            GameObject marker = Instantiate(markerPrefab, worldPos, worldRot, imageTransform);
            marker.name = data.prefabID.ToString();

            var markerDataComponent = marker.GetComponent<MarkerDataComponent>();
            if (markerDataComponent != null)
            {
                markerDataComponent.markerData = data;

            }
            
            saveMarker.markerDatas.Add(data);
            spawnedMarkers.Add(marker);
        }
        
        _isSpawned = true;
    }
} 
