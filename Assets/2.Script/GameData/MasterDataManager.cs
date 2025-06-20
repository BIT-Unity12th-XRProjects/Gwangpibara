using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MasterDataManager : MonoBehaviour
{
    public static MasterDataManager Instance;

    private string _stepDataPath = "StepData";
    private Dictionary<int, StepData> _masterStepDataDictionary;

    private string _itemDataPath = "ItemDataTable";
    private Dictionary<int, ItemData> _masterItemDataDictionary;

    private string _themeDataPath = "ThemeData";
    private Dictionary<int, ThemeData> _masterThemeDataDictionary;

    private Dictionary<int, MapData> _masterMapDataDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            MakeMasterData(); //단계, 주제, 아이템
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
       return GetCopyData(_masterStepDataDictionary, stepID, origin => new StepData(origin));
    }

    public ItemData GetMasterItemData(int itemID)
    {
        return GetCopyData(_masterItemDataDictionary, itemID, origin => new ItemData(origin));
    }

    public Dictionary<int, ThemeData> GetMasterThemeDataDic()
    {
        return _masterThemeDataDictionary;
    }

    //문제 넘버에 맞는 맵 반환
    public MapData GetMasterMapData(int mapID)
    {
        MapData copyMap = new MapData(_masterMapDataDictionary[mapID]);
        
        return copyMap;
    }

    private T GetCopyData<T>(Dictionary<int, T> dictionary, int key, Func<T, T> copyConstructor) where T : class
    {
        if (dictionary.TryGetValue(key, out T value))
        {
            return copyConstructor(value);
        }
        return null;
    }

    private void MakeMasterData()
    {
        _masterStepDataDictionary = MakeMasterData<StepData>(_stepDataPath,
                                value => new StepData(value),
                                dataClass => dataClass.ID);

        _masterItemDataDictionary = MakeMasterData<ItemData>(_itemDataPath,
                                stringValues => new ItemData(stringValues),
                                dataClass => dataClass.ID);

        _masterThemeDataDictionary = MakeMasterData<ThemeData>(_themeDataPath,
                                parseValues => new ThemeData(parseValues),
                                dataclss => dataclss.themeNumber);
    }

    private void MakeMapData()
    {
        _masterMapDataDictionary = new();

        SaveMarkerData loadMarkerData = new();


        List<MarkerData> loadData = loadMarkerData.LoadMarkerList();

        //임시로 1단계 맵 데이터 만들어넣기
        MapData map = new MapData();
        if(loadData.Count != 0)
        {
            map = new(loadData);
        }
        Debug.Log(map.ToString());
        _masterMapDataDictionary.Add(1, map);
    }

  
    private Dictionary<int, T> MakeMasterData<T>(string fileName, Func<string[], T> constructor, Func<T, int> getKey)
        where T : class
    {
        Dictionary<int, T> dictionary = new();
        TextAsset csvFile = Resources.Load<TextAsset>("DataTable/"+ fileName);
        if (csvFile != null)
        {
            string[] lines = csvFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

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

            var imageHandle = Addressables.LoadAssetAsync<Sprite>("ItemImage" + item.ID.ToString());
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
               // Debug.Log(item.ID + "이미지 로드");
                item.itemImage = imageHandle.Result;
            }
            else
            {
                // Debug.LogError($"Prefab Load Fail: {item.ID}");
                item.itemImage = Resources.Load<Sprite>("TestItemImage");
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
                    Debug.LogError($"스프라이트: 디펄트");
                    markerData.markerGameObject = Resources.Load<GameObject>("Image/TestItemPrefab");
                }
            }
        }
    }

}
