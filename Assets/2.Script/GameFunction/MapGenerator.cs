using System.Collections;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    private void Start()
    {
        gameObject.transform.position = Vector3.zero;
    }

    /// <summary>
    /// �� ���� ȣ�� �Լ�
    /// </summary>
    /// <param name="themeNum"></param>
    public IEnumerator C_CallGenerator(int themeNum, Transform mapParent)
    {
        MapData mapData = MasterDataManager.Instance.GetMasterMapData(themeNum);
        Generate(mapData, mapParent);
        yield return null;
    }

    /// <summary>
    /// ��Ŀ�� �����Ѵ�
    /// </summary>
    /// <param name="mapData"></param>
    private void Generate(MapData mapData, Transform mapParent)
    {
        foreach (GameMarkerData markerData in mapData.markerList)
        {
            GameObject newARMarkerObject = Instantiate(markerData.markerGameObject, markerData.position, mapParent.rotation * markerData.rotation, mapParent);
            newARMarkerObject.transform.localScale = markerData.scale;
            newARMarkerObject.AddComponent<ARMarkerObject>().Setting(markerData);
        }
    }
}
