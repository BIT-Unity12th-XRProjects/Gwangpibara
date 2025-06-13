using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MasterDataManager : MonoBehaviour
{
    public static MasterDataManager Instance;
    private Dictionary<int, StepData> masterStepDataDictionary;

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

    private void MakeMasterData()
    {
        masterStepDataDictionary = new();
        string[] lines = File.ReadAllLines("Assets/4.Data/StepData.csv");
        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values[0][0] == '#')
            {
                continue;
            }

            StepData parseItem = new StepData(values);
           // Debug.Log($"파싱한데이터 {parseItem.ID} _ {parseItem.PrintText}");
            masterStepDataDictionary.Add(parseItem.ID, parseItem);
        }
    }

    public Dictionary<int, StepData> GetMasterStepDataDic()
    {
        return masterStepDataDictionary;
    }

    public StepData GetMasterStepData(int stepID)
    {
        return masterStepDataDictionary[stepID];
    }
}
