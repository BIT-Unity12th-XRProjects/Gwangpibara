using System;
using UnityEngine;

public enum ItemType
{
    Clue, Mixture
}

[Serializable]
public class ItemData
{
    public int ID;
    public ItemType ItemType;
    public string Name;
    public string Description;
    public int AquireStep; //단서를 얻을 수 있는 단계 - 현재 플레이 단계보다 이상이면 습득
    public int DeleteStep; //단서가 삭제 될 단계 - 현재 플레이 단계보다 이하이면 삭제
    public GameObject cachedObject; //단서 3D Object
    public Sprite itemImage;

    public ItemData(string[] parseData)
    {
        int idIdx = 0;
        int itemTypeIdx = idIdx + 1;
        int nameIdx = itemTypeIdx + 1;
        int descriptionIdx = nameIdx + 1;
        int acquireStepIdx = descriptionIdx + 1;
        int deleteStepIdx = acquireStepIdx + 1;

        ID = int.Parse(parseData[idIdx]);
        ItemType = EnumParser.ParseEnum<ItemType>(parseData[itemTypeIdx]);
        Name = parseData[nameIdx];
        Description = parseData[descriptionIdx];
        AquireStep = int.Parse(parseData[acquireStepIdx]);
        DeleteStep = int.Parse(parseData[deleteStepIdx]);
        
    }

    public ItemData(ItemData origin)
    {
        ID = origin.ID;
        ItemType = origin.ItemType;
        Name = origin.Name;
        Description = origin.Description;
        AquireStep = origin.AquireStep;
        DeleteStep = origin.DeleteStep;
        cachedObject = origin.cachedObject;
    }

}

