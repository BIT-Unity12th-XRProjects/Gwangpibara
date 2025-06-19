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
    public IEnumerator C_CallGenerator(int themeNum)
    {
        MapData mapData = MasterDataManager.Instance.GetMasterMapData(themeNum);
        Generate(mapData);
        yield return null;
    }

    /// <summary>
    /// ��Ŀ�� �����Ѵ�
    /// </summary>
    /// <param name="mapData"></param>
    private void Generate(MapData mapData)
    {
        foreach (GameMarkerData marker in mapData.markerList)
        {
            Instantiate(marker.markerGameObject, marker.position, marker.rotation, gameObject.transform);
        }
    }
}
