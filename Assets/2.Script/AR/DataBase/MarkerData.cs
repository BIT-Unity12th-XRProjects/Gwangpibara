using UnityEngine;
using System;

[Serializable]
public class MarkerData
{
    public string id;
    public int prefabID;
    public int dropItemID;
    public int acquireStep;
    public int removeStep;
    public Vector3 position;
    public Quaternion rotation;
    public MarkerSpawnType markerSpawnType = MarkerSpawnType.Base;
    public MarkerType markerType = MarkerType.DropItem; 
    
    public MarkerData(string id, 
        int prefabID, 
        int dropItemID, 
        int acquireStep,
        int removeStep,
        Vector3 position,
        Quaternion rotation,
        MarkerSpawnType markerSpawnType = MarkerSpawnType.Base,
        MarkerType markerType = MarkerType.DropItem)
    {
        this.id = id;
        this.prefabID = prefabID;
        this.dropItemID = dropItemID;
        this.markerSpawnType = markerSpawnType;
        this.markerType = markerType;
        this.acquireStep = acquireStep;
        this.removeStep = removeStep;
        this.position = position;
        this.rotation = rotation;
    }

}
