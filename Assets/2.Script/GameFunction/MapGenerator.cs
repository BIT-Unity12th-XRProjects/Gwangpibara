using System.Collections;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    private void Start()
    {
        gameObject.transform.position = Vector3.zero;
    }

    /// <summary>
    /// 맵 생성 호출 함수
    /// </summary>
    /// <param name="themeNum"></param>
    public IEnumerator C_CallGenerator(int themeNum, Transform mapParent)
    {
        MapData mapData = MasterDataManager.Instance.GetMasterMapData(themeNum);
        Generate(mapData, mapParent);
        yield return null;
    }

    /// <summary>
    /// 마커를 생성한다
    /// </summary>
    /// <param name="mapData"></param>
    private void Generate(MapData mapData, Transform mapParent)
    {
        foreach (GameMarkerData markerData in mapData.markerList)
        {
            GameObject newARMarkerObject = Instantiate(markerData.markerGameObject, markerData.position, markerData.rotation, mapParent);
            newARMarkerObject.AddComponent<ARMarkerObject>().Setting(markerData);
        }
    }
}
