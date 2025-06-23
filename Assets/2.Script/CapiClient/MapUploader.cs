using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapUploader : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            MapSaver saver = new();
            SaveMarkerData loadMarkerData = new();
            List<MarkerData> loadData = loadMarkerData.LoadResourceJson("markerdatas");
            saver.UpLoadMapDate(loadData, ParseCurIP.GetLocalIP(), 1234);
        }
    }
}
