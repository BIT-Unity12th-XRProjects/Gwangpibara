using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HeadType
{
    MapDataUpload, MapDataDownload, RoomMake, 
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
        networkManager.Connect(IPAddress.Parse(ParseCurIP.GetLocalIP()), 5000);
        networkManager.OnDataReceived += OnCallBackRecieve;
    }

    private void Update()
    {
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            ReqRoomMake();
        }
    }

    private void OnCallBackRecieve(byte[] recvData)
    {
        HeadType reqType = (HeadType)recvData[0];
        Debug.Log(reqType + "���� ���� " + recvData.Length.ToString());
        HandleReceiveData(reqType, recvData);
    }

    private void HandleReceiveData(HeadType _reqType, byte[] _validData)
    {
        if (_reqType == HeadType.MapDataUpload)
        {
          
        }
        else if(_reqType == HeadType.RoomMake)
        {
            Debug.Log("�����Ŀ ���");
            ResRoomData(_validData);
        }
    }

    private void SendMapDate()
    {
        List<byte> mapReqData = new List<byte>();
        mapReqData.Add((byte)HeadType.MapDataUpload); //��� 
        MapData mapData = MasterDataManager.Instance.GetMasterMapData(1);
        List<GameMarkerData> gameMakerList = mapData.markerList;
        mapReqData.Add((byte)gameMakerList.Count); //��Ŀ�� ���� ��
        for (int i = 0; i < gameMakerList.Count; i++)
        {
            //���Ӹ�Ŀ �����͸� ����Ʈ ȭ
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

            //��ġ ����
            // Vector3 pos (float 3�� = 12����Ʈ)
            mapReqData.AddRange(BitConverter.GetBytes(marker.position.x));
            mapReqData.AddRange(BitConverter.GetBytes(marker.position.y));
            mapReqData.AddRange(BitConverter.GetBytes(marker.position.z));

            // Vector3 rot (float 3�� = 12����Ʈ)
            mapReqData.AddRange(BitConverter.GetBytes(marker.rotation.x));
            mapReqData.AddRange(BitConverter.GetBytes(marker.rotation.y));
            mapReqData.AddRange(BitConverter.GetBytes(marker.rotation.z));
        }
        SendMessege(mapReqData.ToArray());
    }

    private void ReqRoomMake()
    {
        byte[] reqData = { (byte)HeadType.RoomMake };
        SendMessege(reqData);
    }

    private void ResRoomData(byte[] recevData)
    {
        int portNum = BitConverter.ToInt32(recevData, 1);
        Debug.Log("���� ���� ����");
        networkManager.Disconnect();
        Debug.Log("�漭���� ����" +portNum);
        networkManager.Connect(IPAddress.Parse(ParseCurIP.GetLocalIP()), portNum);
    }

    private void SendMessege(byte[] _sendData)
    {
        networkManager.Send(_sendData);
        return;
    }

}
