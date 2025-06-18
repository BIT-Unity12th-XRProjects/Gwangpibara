using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MasterDataManager : MonoBehaviour
{
    public static MasterDataManager Instance;

    private string _stepDataPath = "Assets/4.Data/StepData.csv";
    private Dictionary<int, StepData> _masterStepDataDictionary;

    private string _itemDataPath = "Assets/4.Data/ItemDataTable.csv";
    private Dictionary<int, ItemData> _masterItemDataDictionary;

    private Dictionary<int, MapData> _masterMapDataDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            MakeMasterData();
            MakeMapData();
            StartCoroutine(LoadAllPrefabs());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public StepData GetMasterStepData(int stepID)
    {
        StepData copyData = new StepData(_masterStepDataDictionary[stepID]);
        return copyData;
    }

    public ItemData GetMasterItemData(int itemID)
    {
        ItemData copyItem = new ItemData(_masterItemDataDictionary[itemID]);
        return copyItem;
    }

    //문제 넘버에 맞는 맵 반환
    public MapData GetMasterMapData(int mapID)
    {
        MapData copyMap = new MapData(_masterMapDataDictionary[mapID]);
        return copyMap;
    }

    private void MakeMasterData()
    {
        _masterStepDataDictionary = MakeMasterData<StepData>(_stepDataPath,
                                value => new StepData(value),
                                dataClass => dataClass.ID);

        _masterItemDataDictionary = MakeMasterData<ItemData>(_itemDataPath,
                                stringValues => new ItemData(stringValues),
                                dataClass => dataClass.ID);
    }

    private void MakeMapData()
    {
        _masterMapDataDictionary = new();
        
        //임시로 1단계 맵 데이터 만들어넣기
        MapData mapData = new();
        _masterMapDataDictionary.Add(1, mapData);
    }

    private Dictionary<int, T> MakeMasterData<T>(string path, Func<string[], T> constructor, Func<T, int> getKey)
        where T : class
    {
        Dictionary<int, T> dictionary = new();
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values[0][0] == '#')
            {
                continue;
            }

            T f = constructor(values);
            dictionary.Add(getKey(f), f);
            // Debug.Log(getKey(f) + " 데이터 생성");
        }
        return dictionary;
    }

    private IEnumerator LoadAllPrefabs()
    {
        foreach (var item in _masterItemDataDictionary.Values)
        {
       
            var handle = Addressables.LoadAssetAsync<GameObject>("ItemPrefab"+item.ID.ToString());
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
              //  Debug.Log(item.ID + " 프리팹 로드");
                item.cachedObject = handle.Result;
            }
            else
            {
                // Debug.LogError($"Prefab Load Fail: {item.ID}");
                item.cachedObject = Resources.Load<GameObject>("TestItemPrefab");
            }

        }

        foreach(var item in _masterMapDataDictionary)
        {
            for (int i = 0; i < item.Value.markerList.Count; i++)
            {
                GameMarkerData markerData = item.Value.markerList[i];
                var handle = Addressables.LoadAssetAsync<GameObject>("MarkPrefab" 
                                                    + markerData.markId.ToString());
                yield return handle;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    //  Debug.Log(item.ID + " 프리팹 로드");
                    markerData.markerGameObject = handle.Result;
                }
                else
                {
                    // Debug.LogError($"Prefab Load Fail: {item.ID}");
                    markerData.markerGameObject = Resources.Load<GameObject>("TestItemPrefab");
                }
            }
        }
    }

}
