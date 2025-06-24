using System;
using System.Collections.Generic;
using System.Net;


public class MapDownload
{
    private NetworkManager networkManager;
    public List<GameMarkerData> GameMakerList;

    public void DownloadMapDate(string ip, int mapName)
    {
        networkManager = new NetworkManager();
        networkManager.Connect(IPAddress.Parse("34.22.66.254"), 5000);
        networkManager.OnConnected += () => {

            List<byte> mapReqData = new List<byte>();
            mapReqData.Add((byte)HeadType.MapDataDownload); //헤드 

            mapReqData.AddRange(BitConverter.GetBytes(mapName)); //맵 이름
            networkManager.Send(mapReqData.ToArray());
        } ;
        networkManager.OnDataReceived += OnCallBackRecieve;
    }

    private void OnCallBackRecieve(byte[] data)
    {
        List<GameMarkerData> markers = new List<GameMarkerData>();
        //index 0 은 호출번호
        int index = 1;
        // 마커 수
        int markerCount = BitConverter.ToInt32(data, index);
        index += 4;

        for (int i = 0; i < markerCount; i++)
        {
            GameMarkerData marker = new GameMarkerData();

            marker.markId = BitConverter.ToInt32(data, index);
            index += 4;

            marker.dropItemId = BitConverter.ToInt32(data, index);
            index += 4;

            marker.markerSpawnType = (MarkerSpawnType)data[index++];
            marker.markerType = (MarkerType)data[index++];

            // 문자열 (길이 + UTF-8 문자열)
            byte nameLength = data[index++];
            marker.name = System.Text.Encoding.UTF8.GetString(data, index, nameLength);
            index += nameLength;

            marker.spawnStep = BitConverter.ToInt32(data, index);
            index += 4;

            marker.deleteStep = BitConverter.ToInt32(data, index);
            index += 4;

            
            // Vector3 position
            marker.position.x = BitConverter.ToSingle(data, index); index += 4;
            marker.position.y = BitConverter.ToSingle(data, index); index += 4;
            marker.position.z = BitConverter.ToSingle(data, index); index += 4;

            // Vector3 rotation
            marker.rotation.x = BitConverter.ToSingle(data, index); index += 4;
            marker.rotation.y = BitConverter.ToSingle(data, index); index += 4;
            marker.rotation.z = BitConverter.ToSingle(data, index); index += 4;
            marker.rotation.w = BitConverter.ToSingle(data, index); index += 4;

            // vector3 Scale
            marker.scale.x = BitConverter.ToSingle(data, index); index += 4;
            marker.scale.y = BitConverter.ToSingle(data, index); index += 4;
            marker.scale.z = BitConverter.ToSingle(data, index); index += 4;


            markers.Add(marker);
        }
        GameMakerList = markers;
        networkManager.Disconnect();
    }
}

