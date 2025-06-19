using UnityEngine;
using System;

[Serializable]
public class MarkerData
{
    public string id;
    public string name;
    public int dropItemID;
    public int spawnStep;
    public int deleteStep;
    public Vector3 position;
    public Quaternion rotation;
    public MarkerSpawnType markerSpawnType = MarkerSpawnType.Base;
    public MarkerType markerType = MarkerType.DropItem; 
    
    public MarkerData(string id, string name, Vector3 pos, Quaternion rotation)
    {
        this.id = id;
        this.name = name;
        position = pos;
        this.rotation = rotation;
    }

    public MarkerData(MarkerData markerData)
    {
        id = markerData.id;
        name = markerData.name;
        dropItemID = markerData.dropItemID;
        markerSpawnType = markerData.markerSpawnType;
        markerType = markerData.markerType;
        spawnStep = markerData.spawnStep;
        deleteStep = markerData.deleteStep;
        position = markerData.position;
        rotation = markerData.rotation;
    }
}
