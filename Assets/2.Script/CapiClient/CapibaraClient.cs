using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HeadType
{
    MapData,
}


public class CapibaraClient : MonoBehaviour
{
    public Socket clientSocket;
    public int port = 5000;
    public string ip;
    public int id;
    public string RoomName = "";
    public static IPAddress ServerIp;
    public NetworkManager networkManager;

    private void Start()
    {
        networkManager = new NetworkManager();
        networkManager.Connect();
        networkManager.OnDataReceived += OnCallBackRecieve;
    }

    private void Update()
    {
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            SendMapDate();
        }
    }

    private void OnCallBackRecieve(byte[] recvData)
    {
        HeadType reqType = (HeadType)recvData[0];
        Debug.Log(reqType + "받은 길이 " + recvData.Length.ToString());
        HandleReceiveData(reqType, recvData);
    }

    private void HandleReceiveData(HeadType _reqType, byte[] _validData)
    {
        if (_reqType == HeadType.MapData)
        {
          
        }
    }

    private void SendMapDate()
    {
        List<byte> mapReqData = new List<byte>();
        mapReqData.Add((byte)HeadType.MapData); //헤드 
        MapData mapData = MasterDataManager.Instance.GetMasterMapData(1);
        List<GameMarkerData> gameMakerList = mapData.markerList;
        mapReqData.Add((byte)gameMakerList.Count); //마커들 정보 수
        for (int i = 0; i < gameMakerList.Count; i++)
        {
            //게임마커 데이터를 바이트 화
            GameMarkerData marker = gameMakerList[i];

            // Add integers (4 bytes each)
            mapReqData.AddRange(BitConverter.GetBytes(marker.markId));
            mapReqData.AddRange(BitConverter.GetBytes(marker.dropItemId));
            mapReqData.Add((byte)marker.markerSpawnType); // Assuming enum fits in 1 byte
            mapReqData.Add((byte)marker.markerType);      // Assuming enum fits in 1 byte

            // Add string (length-prefixed)
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(marker.name);
            mapReqData.Add((byte)nameBytes.Length);       // Add string length (assuming < 256)
            mapReqData.AddRange(nameBytes);               // Add string bytes

            // Add remaining integers
            mapReqData.AddRange(BitConverter.GetBytes(marker.spawnStep));
            mapReqData.AddRange(BitConverter.GetBytes(marker.deleteStep));

            //위치 포스
            // Vector3 pos (float 3개 = 12바이트)
            mapReqData.AddRange(BitConverter.GetBytes(marker.position.x));
            mapReqData.AddRange(BitConverter.GetBytes(marker.position.y));
            mapReqData.AddRange(BitConverter.GetBytes(marker.position.z));

            // Vector3 rot (float 3개 = 12바이트)
            mapReqData.AddRange(BitConverter.GetBytes(marker.rotation.x));
            mapReqData.AddRange(BitConverter.GetBytes(marker.rotation.y));
            mapReqData.AddRange(BitConverter.GetBytes(marker.rotation.z));
        }
        SendMessege(mapReqData.ToArray());
    }

    private void SendMessege(byte[] _sendData)
    {
        networkManager.Send(_sendData);
        return;
    }

}
