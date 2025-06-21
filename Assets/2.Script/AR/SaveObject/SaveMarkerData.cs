using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveMarkerData
{
    string path = Application.persistentDataPath + "/markerdatas.json";
    
    public void SaveMarkerList(List<MarkerData> markerDatas)
    {
        MarkerListWrapper wrapper = new MarkerListWrapper();
        wrapper.markerDatas = markerDatas;

        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(path, json);

        Debug.Log("저장 완료: " + path);
    }
    
    public List<MarkerData> LoadMarkerList()
    {
        if (!File.Exists(path))
        {
            return new List<MarkerData>();
        }

        string json = File.ReadAllText(path);
        MarkerListWrapper wrapper = JsonUtility.FromJson<MarkerListWrapper>(json);

        if (wrapper == null || wrapper.markerDatas == null)
        {
            return new List<MarkerData>();
        }
        
        return wrapper.markerDatas;
    }

    public List<MarkerData> LoadResourceJson(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName); // 확장자 제외
        if (jsonFile == null)
        {
            return new List<MarkerData>();
        }

        string json = jsonFile.text;
        MarkerListWrapper wrapper = JsonUtility.FromJson<MarkerListWrapper>(json);

        if (wrapper == null || wrapper.markerDatas == null)
        {
            return new List<MarkerData>();
        }

        return wrapper.markerDatas;
    }
}