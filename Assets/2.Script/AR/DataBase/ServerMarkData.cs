using System;

[Serializable]
public class Vector3Value
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

[Serializable]
public class QuaternionValue
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
}

[Serializable]
public class ServerMarkerData
{
    public int id { get; set; }
    public int prefabID { get; set; }
    public int needItemID { get; set; }
    public int dropItemID { get; set; }
    public int acquireStep { get; set; }
    public int removeStep { get; set; }
    public Vector3Value position { get; set; }
    public QuaternionValue rotation { get; set; }
    public Vector3Value scale { get; set; }
    public MarkerSpawnType markerSpawnType { get; set; }
    public MarkerType markerType { get; set; }

}
