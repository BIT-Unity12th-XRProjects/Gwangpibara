using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class MapSaver
{
    private NetworkManager networkManager;


    public void UpLoadMapDate(List<MarkerData> mapData, string ip, string mapName)
    {
        networkManager = new NetworkManager();
        networkManager.Connect(IPAddress.Parse("34.22.66.254"), 5000);
        networkManager.OnConnected += () => {

            List<byte> mapReqData = new List<byte>();
            mapReqData.Add((byte)HeadType.MapDataUpload); //헤드 

            int mapNumber = 1234;
            int.TryParse(mapName, out mapNumber);
            if(mapNumber == 0)
            {
                mapNumber = 1234;
            }
            
            mapReqData.AddRange(BitConverter.GetBytes(mapNumber)); //맵 이름

            List<MarkerData> gameMakerList = mapData;
            mapReqData.AddRange(BitConverter.GetBytes(gameMakerList.Count)); //마커들 정보 수
            for (int i = 0; i < gameMakerList.Count; i++)
            {
                //게임마커 데이터를 바이트 화
                MarkerData marker = gameMakerList[i];

                // Add integers (4 bytes each)
                mapReqData.AddRange(BitConverter.GetBytes(marker.prefabID));
                mapReqData.AddRange(BitConverter.GetBytes(marker.dropItemID));
                mapReqData.Add((byte)marker.markerSpawnType); // Assuming enum fits in 1 byte
                mapReqData.Add((byte)marker.markerType);      // Assuming enum fits in 1 byte

                // Add string (length-prefixed)
                byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes("테스트");
                mapReqData.Add((byte)nameBytes.Length);       // Add string length (assuming < 256)
                mapReqData.AddRange(nameBytes);               // Add string bytes

                // Add remaining integers
                mapReqData.AddRange(BitConverter.GetBytes(marker.acquireStep));
                mapReqData.AddRange(BitConverter.GetBytes(marker.removeStep));

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

            networkManager.Send(mapReqData.ToArray());
        } ;
        networkManager.OnDataReceived += OnCallBackRecieve;
    }

    private void OnCallBackRecieve(byte[] data)
    {
        networkManager.Disconnect();
    }
}

