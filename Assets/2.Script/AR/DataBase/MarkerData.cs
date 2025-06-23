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
    public Vector3 scale;
    public MarkerSpawnType markerSpawnType;
    public MarkerType markerType;
}
