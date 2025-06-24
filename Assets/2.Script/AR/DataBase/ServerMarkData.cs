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
    public int dropItemID { get; set; }
    public int acquireStep { get; set; }
    public int removeStep { get; set; }
    public Vector3Value position { get; set; }
    public QuaternionValue rotation { get; set; }
    public Vector3Value scale { get; set; }
    public MarkerSpawnType markerSpawnType { get; set; }
    public MarkerType markerType { get; set; }

    public ServerMarkerData(MarkerData markerData)
    {
         this.prefabID = markerData.prefabID;
         this.dropItemID = markerData.dropItemID;
         this.acquireStep = markerData.acquireStep;
         this.removeStep = markerData.removeStep;
         
         Vector3Value positionValue = new Vector3Value();
         positionValue.X = markerData.position.x;
         positionValue.Y = markerData.position.y;
         positionValue.Z = markerData.position.z;
         
         QuaternionValue rotationValue = new QuaternionValue();
         rotationValue.X = markerData.rotation.x;
         rotationValue.Y = markerData.rotation.y;
         rotationValue.Z = markerData.rotation.z;
         
         Vector3Value scaleValue = new Vector3Value();
         scaleValue.X = markerData.scale.x;
         scaleValue.Y = markerData.scale.y;
         scaleValue.Z = markerData.scale.z;
         
         this.position = positionValue;
         this.rotation = rotationValue;
         this.scale = scaleValue;
         
         this.markerSpawnType = markerData.markerSpawnType;
         this.markerType = markerData.markerType;
         
         
    }


}
