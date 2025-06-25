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
    
    [SerializeField] private MarkersApiClient _markersApiClient;

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

        MarkerData[] markerDatasArray;

        StartCoroutine(_markersApiClient.GetAllMarkers(
            markers =>
            {
                markerDatasArray = ChangeServerDataToMarkerData(markers);
                RePositionMarker(markerDatasArray, imageTransform);
            },
            err => { }
        ));
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

    private void RePositionMarker(MarkerData[] markerDatasArray, Transform imageTransform)
    {
        foreach (var data in markerDatasArray)
        {
            Vector3 worldPos = imageTransform.TransformPoint(data.position);
            Quaternion worldRot = imageTransform.rotation * data.rotation;

            GameObject marker = Instantiate(markerPrefab, worldPos, worldRot, imageTransform);
            marker.name = data.prefabID.ToString();
            marker.transform.localScale = data.scale;

            var markerDataComponent = marker.GetComponent<MarkerDataComponent>();
            if (markerDataComponent != null)
            {
                markerDataComponent.markerData = data;
            }
        }
    }

    private MarkerData[] ChangeServerDataToMarkerData(ServerMarkerData[] loadServerMarkerList)
    {
        List<MarkerData> markerDataList = new();
        
        foreach (var marker in loadServerMarkerList)
        {
            MarkerData markerData = new MarkerData();
            markerData.prefabID = marker.prefabID;
            markerData.needItemID = marker.needItemID;
            markerData.dropItemID = marker.dropItemID;
            markerData.acquireStep = marker.acquireStep;
            markerData.removeStep = marker.removeStep;
            
            Vector3 positionValue = new Vector3();
            positionValue.x = marker.position.X;
            positionValue.y = marker.position.Y;
            positionValue.z = marker.position.Z;
         
            Quaternion rotationValue = new Quaternion();
            rotationValue.x = marker.rotation.X;
            rotationValue.y = marker.rotation.Y;
            rotationValue.z = marker.rotation.Z;
            rotationValue.w = marker.rotation.W;
         
            Vector3 scaleValue = new Vector3();
            scaleValue.y = marker.scale.Y;
            scaleValue.z = marker.scale.Z;
            scaleValue.x = marker.scale.X;
            
            markerData.position = positionValue;
            markerData.rotation = rotationValue;
            markerData.scale = scaleValue;
         
            markerData.markerSpawnType = marker.markerSpawnType;
            markerData.markerType = marker.markerType;
            
            markerDataList.Add(markerData);
        }
        
        return markerDataList.ToArray();
    }
    
    
} 
