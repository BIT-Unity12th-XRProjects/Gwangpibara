using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

public class SavePosition : MonoBehaviour
{
    public List<MarkerData> markerDatas;
    [SerializeField] private ARMarkerSpawner _arMarkerSpawner;
    [SerializeField] private SearchPosition _searchPosition;
    
    private void OnEnable()
    {
        ARMarkerSpawner.OnPositionDebug += UpdateMarkerData;
        RenameEvents.OnMarkerRenamed += HandleRename;
    }

    private void OnDisable()
    {
        ARMarkerSpawner.OnPositionDebug -= UpdateMarkerData;
        RenameEvents.OnMarkerRenamed -= HandleRename;
    }

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
            existing.objectName = objectName;
        }
        else
        {
            MarkerData objectMarker = new MarkerData(id, objectName, localPos, localRot);
            markerDatas.Add(objectMarker);
        }
    }

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
                existing.objectName = marker.objectName;
                existing.rotation = marker.rotation;
                existing.position = marker.position;
            }
            else
            {
                loadMarkerList.Add(marker);
            }
        }

        markerDataHandler.SaveMarkerList(loadMarkerList);
        Debug.Log(loadMarkerList.Count);
    }
    
    public void RemoveMarkerData(string markerId)
    {
        markerDatas.RemoveAll(m => m.id == markerId);
    }
    
    
    private void HandleRename(string oldName, string newName)
    {
        foreach (var marker in markerDatas)
        {
            if (marker.objectName == oldName)
            {
                marker.objectName = newName;
                break;
            }
        }
    }
}
