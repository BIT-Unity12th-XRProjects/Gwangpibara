using System.Collections.Generic;
using AREditor.LoadObject;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MarkerLoader : MonoBehaviour
{ 
    [SerializeField] private GameObject markerPrefab;
    private SaveMarkerData _saveMarkerData;
    [SerializeField] private SavePosition savePosition;
    
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
            marker.name = data.objectName;

            var idHolder = marker.GetComponent<MarkerIDHolder>();
            if (idHolder != null)
            {
                idHolder.markerId = data.id;

            }
            savePosition.markerDatas.Add(data);
            
            spawnedMarkers.Add(marker);
        }
        
        _isSpawned = true;
    }
} 
