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
                existing.dropItemID = marker.dropItemID;
                existing.acquireStep = marker.acquireStep;
                existing.removeStep = marker.removeStep;
                existing.markerSpawnType = marker.markerSpawnType;
                existing.markerType = marker.markerType;
                existing.position = marker.position;
                existing.rotation = marker.rotation;
            }
            else
            {
                loadMarkerList.Add(marker);
            }
        }

        markerDataHandler.SaveMarkerList(loadMarkerList, fileName);
        Debug.Log(loadMarkerList.Count);
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
}
