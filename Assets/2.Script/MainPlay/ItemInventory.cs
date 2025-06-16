
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInventory
{
    [SerializeField] private List<ItemData> _haveItemList;

    public ItemInventory()
    {
        _haveItemList = new();
    }

    public bool CheckAcquireItemCondition(ItemData itemData, int progressedStep)
    {
        //습득하려는 아이템이 습득가능한 상태인지
        if (progressedStep < itemData.AquireStep)
        {
            //진행한 단계가 습득단계 아래면 습득 불가
            return false;
        }

        //중복 아이템인지
        if (GetItemAmount(itemData) != 0)
        {
            return false;
        }

        return true;
    }


    public void AddItem(ItemData itemData)
    {
        _haveItemList.Add(itemData);
    }

    public int GetItemAmount(ItemData itemData)
    {
        int haveAmount = 0;
        for (int i = 0; i < _haveItemList.Count; i++)
        {
            if (_haveItemList[i].ID == itemData.ID)
            {
                haveAmount += 1;
            }
        }

        return haveAmount;
    }

    public int GetItemAmount(int findID)
    {
        int haveAmount = 0;
        for (int i = 0; i < _haveItemList.Count; i++)
        {
            if (_haveItemList[i].ID == findID)
            {
                haveAmount += 1;
            }
        }

        return haveAmount;
    }
}

