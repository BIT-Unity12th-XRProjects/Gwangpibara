using UnityEngine;
using System;

[System.Serializable]
public class MarkerData
{
    public string id;
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
    
    public MarkerData(string objectName, Vector3 position)
    {
        id = Guid.NewGuid().ToString();
        this.objectName = objectName;
        this.position = position;
    }
    
    public MarkerData(string id,string name, Vector3 pos, Quaternion rotation)
    {
        this.id = id;
        objectName = name;
        position = pos;
        this.rotation = rotation;
    }
}
