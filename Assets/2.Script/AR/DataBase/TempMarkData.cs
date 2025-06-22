using System;

[Serializable]
public struct Vector3Value
{
    public float X, Y, Z;
    public Vector3Value(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
[Serializable]
public struct QuaternionValue
{
    public float X, Y, Z, W;

    public QuaternionValue(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
}

[Serializable]
public class TempMarkerData
{
    public int id;
    public int prefabID;
    public int dropItemID;
    public int acquireStep;
    public int removeStep;
    public Vector3Value position;
    public QuaternionValue rotation;
    public MarkerSpawnType markerSpawnType = MarkerSpawnType.Base;
    public MarkerType markerType = MarkerType.DropItem;

    public TempMarkerData(
        int id,
        int prefabID,
        int dropItemID,
        int acquireStep,
        int removeStep,
        Vector3Value position,
        QuaternionValue rotation,
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
