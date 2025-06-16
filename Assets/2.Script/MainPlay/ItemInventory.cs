
using System.Collections.Generic;

public class ItemInventory
{
    private List<ItemData> _haveItemList;

    public ItemInventory()
    {
        _haveItemList = new();
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

