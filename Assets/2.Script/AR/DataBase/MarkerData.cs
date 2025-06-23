using UnityEngine;
using System;
using UnityEngine.UIElements;


[Serializable]
public class MarkerData
{
    public string ID;
    public int PrefabID;
    public int DropItemID;
    public int AcquireStep;
    public int RemoveStep;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public MarkerSpawnType MarkerSpawnType = MarkerSpawnType.Base;
    public MarkerType MarkerType = MarkerType.DropItem; 
    
    public MarkerData(
        string id, 
        int prefabID, 
        int dropItemID, 
        int acquireStep,
        int removeStep,
        Vector3 position,
        Quaternion rotation,
        Vector3 scale,
        MarkerSpawnType markerSpawnType = MarkerSpawnType.Base,
        MarkerType markerType = MarkerType.DropItem)
    {
        this.ID = id;
        this.PrefabID = prefabID;
        this.DropItemID = dropItemID;
        this.MarkerSpawnType = markerSpawnType;
        this.MarkerType = markerType;
        this.AcquireStep = acquireStep;
        this.RemoveStep = removeStep;
        this.Position = position;
        this.Rotation = rotation;
        this.Scale = scale;
    }

}
