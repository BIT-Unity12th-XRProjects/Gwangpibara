using System.Collections.Generic;
using System.Linq;
using AREditor.LoadObject;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MarkerLoader : MonoBehaviour
{ 
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private SaveMarker saveMarker;
    private SaveMarkerData _saveMarkerData;
    private List<GameObject> spawnedMarkers = new List<GameObject>();
    private string _lastLoadedFileName = null;

    private void Awake()
    {
        _saveMarkerData = new SaveMarkerData();
    }
    
    public void LoadAndSpawnMarkers(Transform imageTransform, string fileName)
    {
        if (_lastLoadedFileName == fileName)
        {
            return;
        } 
        
        Debug.Log(fileName +"이 로드됨");
        
        foreach (var obj in spawnedMarkers)
        {
            Destroy(obj);
        }
        spawnedMarkers.Clear();
        saveMarker.markerDatas.Clear();
    
        List<MarkerData> markerDatas = _saveMarkerData.LoadMarkerList(fileName);
        
        foreach (var data in markerDatas)
        {
            Vector3 worldPos = imageTransform.TransformPoint(data.Position);
            Quaternion worldRot = imageTransform.rotation * data.Rotation;
            
            GameObject marker = Instantiate(markerPrefab, worldPos, worldRot, imageTransform);
            marker.name = data.PrefabID.ToString();
            
            Transform child = marker.transform.Find("MarkerRenderer");
            if (child != null)
            {
                child.localScale = data.Scale;
            }

            var markerDataComponent = marker.GetComponent<MarkerDataComponent>();
            if (markerDataComponent != null)
            {
                markerDataComponent.markerData = data;

            }
            
            saveMarker.markerDatas.Add(data);
            spawnedMarkers.Add(marker);
        }
        _lastLoadedFileName = fileName;
    }
    
    // 불러오기된 마커 전부 삭제하는 버튼
    public void ResetAllMarkers()
    {
        foreach (var marker in spawnedMarkers)
        {
            Destroy(marker);
        }
        spawnedMarkers.Clear();
        saveMarker.markerDatas.Clear();
    
        _lastLoadedFileName = null;
    }
    
} 
