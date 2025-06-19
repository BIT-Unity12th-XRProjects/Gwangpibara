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
    
    private void OnEnable()
    {
        ARMarkerSpawner.OnPositionDebug += UpdateMarkerData;
        UpdateEvents.OnMarkerStringDataUpdated += HandleRename;
        UpdateEvents.OnMarkerIntDataUpdated += HandleDropID;
        UpdateEvents.OnMarkerIntDataUpdated += HandleSpawnStep;
        UpdateEvents.OnMarkerIntDataUpdated += HandleDeleteStep;
        UpdateEvents.OnMarkerMarkerSpawnTypeDataUpdated += HandleMarkerSpawnType;
        UpdateEvents.OnMarkerMarkerTypeDataUpdated += HandleMarkerType;
        
    }

    private void OnDisable()
    {
        ARMarkerSpawner.OnPositionDebug -= UpdateMarkerData;
        UpdateEvents.OnMarkerStringDataUpdated -= HandleRename;
        UpdateEvents.OnMarkerIntDataUpdated -= HandleDropID;
        UpdateEvents.OnMarkerIntDataUpdated -= HandleSpawnStep;
        UpdateEvents.OnMarkerIntDataUpdated -= HandleDeleteStep;
        UpdateEvents.OnMarkerMarkerSpawnTypeDataUpdated -= HandleMarkerSpawnType;
        UpdateEvents.OnMarkerMarkerTypeDataUpdated -= HandleMarkerType;
    }
    
    // 마커의 위치 업데이트
    private void UpdateMarkerData(Vector3 position, Quaternion rotation, string objectName, string id)
    {
        Transform trackedImageTransform = _searchPosition.GetTrackedImageTransform();
        if (trackedImageTransform == null)
        {
            return;
        }
        
        Vector3 localPos = trackedImageTransform.InverseTransformPoint(position);
        Quaternion localRot = Quaternion.Inverse(trackedImageTransform.rotation) * rotation;

        var existing = markerDatas.FirstOrDefault(m => m.id == id);
        if (existing != null)
        {
            existing.position = localPos;
            existing.rotation = localRot;
            existing.name = objectName;
        }
        else
        {
            MarkerData objectMarker = new MarkerData(id, objectName, localPos, localRot);
            markerDatas.Add(objectMarker);
        }
    }

    // 마커 데이터 저장 (저장버튼)
    public void SaveMarkerPosition()
    {
        SaveMarkerData markerDataHandler = new SaveMarkerData();
        List<MarkerData> loadMarkerList = markerDataHandler.LoadMarkerList();

        var currentIds = new HashSet<string>(markerDatas.Select(m => m.id));

        loadMarkerList.RemoveAll(m => !currentIds.Contains(m.id));
        
        foreach (var marker in markerDatas)
        {
            var existing = loadMarkerList.FirstOrDefault(m => m.id == marker.id);

            if (existing != null)
            {
                existing.name = marker.name;
                existing.dropItemID = marker.dropItemID;
                existing.rotation = marker.rotation;
                existing.position = marker.position;
                existing.spawnStep = marker.spawnStep;
                existing.deleteStep = marker.deleteStep;
                existing.markerSpawnType = marker.markerSpawnType;
                existing.markerType = marker.markerType;
                
            }
            else
            {
                loadMarkerList.Add(marker);
            }
        }

        markerDataHandler.SaveMarkerList(loadMarkerList);
        Debug.Log(loadMarkerList.Count);
    }
    
    // 마커 데이터 삭제(Eraser 버튼)
    public void RemoveMarkerData(string markerId)
    {
        markerDatas.RemoveAll(m => m.id == markerId);
    }
    
    // 이름 변경
    private void HandleRename(string oldName, string newName)
    {
        foreach (var marker in markerDatas)
        {
            if (marker.name == oldName)
            {
                marker.name = newName;
            }
        }
    }
    
    // DropID 변경
    private void HandleDropID(int oldID, int newID)
    {
        foreach (var marker in markerDatas)
        {
            if (marker.dropItemID == oldID)
            {
                marker.dropItemID = newID;
            }
        }
    }

    // SpawnType 변경
    private void HandleSpawnStep(int oldSpawnType, int newSpawnType)
    {
        foreach (var marker in markerDatas)
        {
            if (marker.spawnStep == oldSpawnType)
            {
                marker.spawnStep = newSpawnType;
            }
        }
    }

    // DeleteStep 변경
    private void HandleDeleteStep(int oldDeleteStep, int newDeleteStep)
    {
        foreach (var marker in markerDatas)
        {
            if (marker.deleteStep == oldDeleteStep)
            {
                marker.deleteStep = newDeleteStep;
            }
        }
    }

    // MarkerSpawnType 변경
    private void HandleMarkerSpawnType(MarkerSpawnType oldMarkerSpawnType, MarkerSpawnType newMarkerSpawnType)
    {
        foreach (var marker in markerDatas)
        {
            if (marker.markerSpawnType == oldMarkerSpawnType)
            {
                marker.markerSpawnType = newMarkerSpawnType;
            }
        }
    }
    
    // MarkerType 변경
    private void HandleMarkerType(MarkerType oldMarkerType, MarkerType newMarkerType)
    {
        foreach (var marker in markerDatas)
        {
            if (marker.markerType == oldMarkerType)
            {
                marker.markerType = newMarkerType;
            }
        }
    }

}
