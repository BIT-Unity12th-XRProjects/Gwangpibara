using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : BaseUI
{
    private Action m_onExitBtnClicked;
    private List<ItemSlot> itemSlotList = new();
    [SerializeField] private ItemSlot itemSlotPrefab;
    [SerializeField] private Transform slotTransform;

    public override void SetInfo(BaseUIData uiData)
    {
        List<ItemData> itemList = MainController.Instance.GetItemInventory().GetItemList();
        int slotCount = itemSlotList.Count; //현재 보유중인 슬롯 수 
        int itemCount = itemList.Count; //보여야할 아이템 수

        for (int i = 0; i < itemCount; i++)
        {
            if(i < slotCount)
            {
                itemSlotList[i].gameObject.SetActive(true);
                itemSlotList[i].SetSlot(itemList[i]);
                continue;
            }
            else
            {
                //슬롯 부족하면 만들기
                ItemSlot slot = Instantiate(itemSlotPrefab, slotTransform);
                itemSlotList.Add(slot);
                slot.gameObject.SetActive(true);
                slot.SetSlot(itemList[i]);
                break;
            }
        }

        for(int i = itemCount; i < itemSlotList.Count; i++)
        {
            itemSlotList[i].gameObject.SetActive(false);   
        }
    }

    public void OnClickedExitButton()
    {
        m_onExitBtnClicked?.Invoke();
        CloseUI(true);
    }
}
