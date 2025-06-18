using System;
using System.Collections.Generic;

[Serializable]
public class MapData
{
    public List<GameMarkerData> markerList;

    public MapData()
    {
        markerList = new();
        Random ran = new Random();

        for (int i = 0; i < 7; i++)
        {
            float x = ran.Next(-3, 3);
            float y = ran.Next(0, 2);
            float z = ran.Next(-3, 3);
            GameMarkerData testData = new GameMarkerData(10101, new UnityEngine.Vector3(x, y, z));
            markerList.Add(testData);
        }
        
    }

    public MapData(MapData origin)
    {
        markerList = new List<GameMarkerData>();
        for (int i = 0; i < origin.markerList.Count; i++)
        {
            GameMarkerData copyMarkerData = new GameMarkerData(origin.markerList[i]);
            markerList.Add(copyMarkerData);
        }
    }
}

