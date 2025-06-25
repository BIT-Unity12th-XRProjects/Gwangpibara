using System;
using UnityEngine;
public enum MarkerType
{
    DropItem, Clue, SelfClue, Decoration, Wall
}

public enum MarkerSpawnType
{
    Base, OnClose
}

[Serializable]
public class GameMarkerData
{
    public int markId;
    public string name;
    public int needItemId;
    public int dropItemId;
    public int spawnStep;
    public int deleteStep;
    public Vector3 position;
    public Quaternion rotation;
    public MarkerSpawnType markerSpawnType;
    public MarkerType markerType;
    public Vector3 scale;
    public GameObject markerGameObject;

    public GameMarkerData()
    {
        
    }

    public GameMarkerData(int id, Vector3 spawnPosition)
    {
        markId = id;
        name = "테스트";
        spawnStep = 10101;
        deleteStep = 99999;
        position = spawnPosition;
    }

    public GameMarkerData(ServerMarkerData serverData)
    {
        markId = serverData.prefabID;
        name = $"Marker_테스트 이름"; // 또는 필요시 다른 방식으로 이름 지정
        needItemId = serverData.needItemID;
        dropItemId = serverData.dropItemID;
        spawnStep = serverData.acquireStep;
        deleteStep = serverData.removeStep;

        position = new Vector3(
            serverData.position.X,
            serverData.position.Y,
            serverData.position.Z
        );

        rotation = new Quaternion(
            serverData.rotation.X,
            serverData.rotation.Y,
            serverData.rotation.Z,
            serverData.rotation.W
        );

        scale = new Vector3(
            serverData.scale.X,
            serverData.scale.Y,
            serverData.scale.Z
        );

        markerSpawnType = serverData.markerSpawnType;
        markerType = serverData.markerType;
    }

    public GameMarkerData(MarkerData markerData)
    {
        //MarkderData의 name은 GamMarkerData에서 ID
        markId = markerData.prefabID; // 프리팹 껍데기 ID
        needItemId = markerData.needItemID;
         dropItemId = markerData.dropItemID;
        markerSpawnType = markerData.markerSpawnType;
        markerType = markerData.markerType;
        name = "테스트 이름"; //이건 마커 오브젝트의 이름 - 
        spawnStep = markerData.acquireStep;
        deleteStep = markerData.removeStep;
        position = markerData.position;
        rotation = markerData.rotation;
        scale = markerData.scale;
    }

    public GameMarkerData(GameMarkerData origin)
    {
        markId = origin.markId;
        markerSpawnType = origin.markerSpawnType;
        markerType = origin.markerType;
        needItemId = origin.needItemId;
        dropItemId = origin.dropItemId;
        name = origin.name;
        spawnStep = origin.spawnStep;
        deleteStep = origin.deleteStep;
        position = origin.position;
        rotation = origin.rotation;
        scale = origin.scale;
        markerGameObject = origin.markerGameObject; //GameObject는 참조 복사입니다.
    }
}

