﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MasterDataManager : Singleton<MasterDataManager>
{

    private Dictionary<int, StepData> _masterStepDataDictionary;
    private Dictionary<int, ItemData> _masterItemDataDictionary;
    private Dictionary<int, ThemeData> _masterThemeDataDictionary;
    private Dictionary<int, MapData> _masterMapDataDictionary;

    public IEnumerator SetData()
    {
        Debug.Log("데이터");
       MakeMasterData(); //단계, 주제, 아이템
       yield return StartCoroutine(LoadAllPrefabs());
    }

    public StepData GetMasterStepData(int stepID)
    {
       return GetCopyData(_masterStepDataDictionary, stepID, origin => new StepData(origin));
    }

    public ItemData GetMasterItemData(int itemID)
    {
        return GetCopyData(_masterItemDataDictionary, itemID, origin => new ItemData(origin));
    }

    public ThemeData GetMasterThemeData(int themeNumber)
    {
        return GetCopyData(_masterThemeDataDictionary, themeNumber, origin => new ThemeData(origin));
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
      
        _masterStepDataDictionary = MakeMasterData<StepData>(EMasterData.StepData,
                        value => new StepData(value),
                        dataClass => dataClass.ID);

        _masterItemDataDictionary = MakeMasterData<ItemData>(EMasterData.ItemData,
                                stringValues => new ItemData(stringValues),
                                dataClass => dataClass.ID);

        _masterThemeDataDictionary = MakeMasterData<ThemeData>(EMasterData.ThemeData,
                                parseValues => new ThemeData(parseValues),
                                dataclss => dataclss.themeNumber);

        _masterMapDataDictionary = new();
    }

     private Dictionary<int, T> MakeMasterData<T>(EMasterData masterDataType, Func<string[], T> constructor, Func<T, int> getKey)
          where T : class
    {
        Dictionary<int, T> dictionary = new();
        List<string[]> parsings = ParsingManager.Instance.GetMasterData(masterDataType).DbValueList;

        for (int i = 0; i < parsings.Count; i++)
        {
            string[] values = parsings[i];
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

            var imageHandle = Addressables.LoadAssetAsync<Sprite>("ItemImage" + item.ID.ToString());
            yield return imageHandle;
            if (imageHandle.Status == AsyncOperationStatus.Succeeded)
            {
              //  Debug.Log(item.ID + "이미지 로드");
                item.itemImage = imageHandle.Result;
            }
            else
            {
              //  Debug.LogError($"Image Load Fail: {item.ID}");
                item.itemImage = Resources.Load<Sprite>("Image/TestItemImage");
            }

        }

    }

    public IEnumerator DownLoadMap(List<GameMarkerData> markList, int mapName)
    {
        MapData map = new MapData();
        map.markerList = markList;
        for (int i = 0; i < markList.Count; i++)
        {
            GameMarkerData markerData = markList[i];
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
                Debug.LogError($"게임오브젝트: 디펄트");
                markerData.markerGameObject = Resources.Load<GameObject>("TestItemPrefab");
            }
        }
        if(_masterMapDataDictionary.ContainsKey(mapName) == false)
        {
            _masterMapDataDictionary.Add(mapName, null);
        }
        _masterMapDataDictionary[mapName] = map;
    }
}
