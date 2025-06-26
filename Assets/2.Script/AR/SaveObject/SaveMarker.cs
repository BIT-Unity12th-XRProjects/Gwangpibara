using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

public class SaveMarker : MonoBehaviour
{
    public List<MarkerData> markerDatas;
    [SerializeField] private ARMarkerSpawner _arMarkerSpawner;
    [SerializeField] private SearchPosition _searchPosition;
    [SerializeField] private MarkersApiClient _markersApiClient;
    
    // 마커 데이터 저장 (저장버튼)
    public void SaveMarkerPosition(string fileName)
    {
        SaveMarkerData markerDataHandler = new SaveMarkerData();
        List<MarkerData> loadMarkerList = markerDataHandler.LoadMarkerList(fileName);

        var currentIds = new HashSet<string>(markerDatas.Select(m => m.id));

        loadMarkerList.RemoveAll(m => !currentIds.Contains(m.id));
        
        foreach (var marker in markerDatas)
        {
            var existing = loadMarkerList.FirstOrDefault(m => m.id == marker.id);

            if (existing != null)
            {
                existing.prefabID = marker.prefabID;
                existing.needItemID = marker.needItemID;
                existing.dropItemID = marker.dropItemID;
                existing.acquireStep = marker.acquireStep;
                existing.removeStep = marker.removeStep;
                existing.markerSpawnType = marker.markerSpawnType;
                existing.markerType = marker.markerType;
                existing.position = marker.position;
                existing.rotation = marker.rotation;
                existing.scale = marker.scale;
            }
            else
            {
                loadMarkerList.Add(marker);
            }
        }
        
        //로컬 저장
        markerDataHandler.SaveMarkerList(loadMarkerList, fileName);
        
        //db 저장
        List<ServerMarkerData> serverMarkerDatas = ChangeMarkerDataToServerData(markerDatas);
        StartCoroutine(_markersApiClient.UpdateMarkersBulk(serverMarkerDatas.ToArray()));

    }
    
    // 수정된 마크 데이터 업데이트
    public void UpdateMarkerDataInList(MarkerData updatedData)
    {
        Transform trackedImageTransform = _searchPosition.GetTrackedImageTransform();
        if (trackedImageTransform == null)
        {
            return;
        }
        
        Vector3 localPos = trackedImageTransform.InverseTransformPoint(updatedData.position);
        Quaternion localRot = Quaternion.Inverse(trackedImageTransform.rotation) * updatedData.rotation;
        
        updatedData.position = localPos;
        updatedData.rotation = localRot;
        

        Debug.Log(localPos);
        Debug.Log(localRot);
        
        for (int i = 0; i < markerDatas.Count; i++)
        {
            if (markerDatas[i].id == updatedData.id)
            {
                markerDatas[i] = updatedData;
                return;
            }
        }
        markerDatas.Add(updatedData);
    }
    
    // 마커 데이터 삭제(Eraser 버튼)
    public void RemoveMarkerData(string markerId)
    {
        markerDatas.RemoveAll(m => m.id == markerId);
    }

    private List<ServerMarkerData> ChangeMarkerDataToServerData(List<MarkerData> loadMarkerList)
    {
        List<ServerMarkerData> serverMarkerDataList = new();
        foreach (var marker in loadMarkerList)
        {
            ServerMarkerData serverMarkerData = new ServerMarkerData();
            serverMarkerData.prefabID = marker.prefabID;
            serverMarkerData.needItemID = marker.needItemID;
            serverMarkerData.dropItemID = marker.dropItemID;
            serverMarkerData.acquireStep = marker.acquireStep;
            serverMarkerData.removeStep = marker.removeStep;
            
            Vector3Value positionValue = new Vector3Value();
            positionValue.X = marker.position.x;
            positionValue.Y = marker.position.y;
            positionValue.Z = marker.position.z;
         
            QuaternionValue rotationValue = new QuaternionValue();
            rotationValue.X = marker.rotation.x;
            rotationValue.Y = marker.rotation.y;
            rotationValue.Z = marker.rotation.z;
            rotationValue.W = marker.rotation.w;
         
            Vector3Value scaleValue = new Vector3Value();
            scaleValue.X = marker.scale.x;
            scaleValue.Y = marker.scale.y;
            scaleValue.Z = marker.scale.z;
            
            serverMarkerData.position = positionValue;
            serverMarkerData.rotation = rotationValue;
            serverMarkerData.scale = scaleValue;
         
            serverMarkerData.markerSpawnType = marker.markerSpawnType;
            serverMarkerData.markerType = marker.markerType;
            
            serverMarkerDataList.Add(serverMarkerData);
        }
        
        return serverMarkerDataList;
    }
}
