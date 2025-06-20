

using System;
using UnityEngine;

public enum MarkerType
{
    DropItem, Clue, SelfClue, Decoration, Trap 
}

public enum MarkerSpawnType
{
    Base, OnClose
}

[Serializable]
public class GameMarkerData
{
    public int markId; 
    public MarkerSpawnType markerSapwnType = MarkerSpawnType.Base;
    public MarkerType markerType = MarkerType.DropItem;
    public string name;
    public int spawnStep;
    public int deleteStep;
    public Vector3 position;
    public Quaternion rotation;
    public GameObject markerGameObject;

    public GameMarkerData(int id, Vector3 spawnPosition)
    {
        markId = id;
        name = "테스트";
        spawnStep = 10101;
        deleteStep = 99999;
        position = spawnPosition;
    }

    public GameMarkerData(MarkerData markerData)
    {

    }

    public GameMarkerData(GameMarkerData origin)
    {
        markId = origin.markId;
        markerSapwnType = origin.markerSapwnType;
        markerType = origin.markerType;
        name = origin.name;
        spawnStep = origin.spawnStep;
        deleteStep = origin.deleteStep;
        position = origin.position;
        rotation = origin.rotation;
        markerGameObject = origin.markerGameObject; //GameObject는 참조 복사입니다.
    }
}

