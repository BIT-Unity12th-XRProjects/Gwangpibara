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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            MakeMasterData();
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

    private void MakeMasterData()
    {
        _masterStepDataDictionary = MakeMasterData<StepData>(_stepDataPath,
                                value => new StepData(value),
                                dataClass => dataClass.ID);

        _masterItemDataDictionary = MakeMasterData<ItemData>(_itemDataPath,
                                stringValues => new ItemData(stringValues),
                                dataClass => dataClass.ID);
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
            var a = Addressables.ResourceLocators;

            //어드레서블에 해당 id의 오브젝트 키가 있는지 체크 - keyException 방지

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
    }

}
