using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public StepData GetMasterStepData(int stepID)
    {
        return _masterStepDataDictionary[stepID];
    }

    public ItemData GetMasterItemData(int itemID)
    {
        return _masterItemDataDictionary[itemID];
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

}
