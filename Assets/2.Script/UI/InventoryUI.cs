using System;
using UnityEngine;

public class InventoryUIData : BaseUIData
{
    public Action onExitBtnClicked;

    //ui에 필요한 데이터 추가...
}

public class InventoryUI : BaseUI
{
    private Action m_onExitBtnClicked;

    private InventoryUIData m_inventoryUIData;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_inventoryUIData = uiData as InventoryUIData;

        m_onExitBtnClicked = m_inventoryUIData.onExitBtnClicked;
    }

    public void OnClickedExitButton()
    {
        m_onExitBtnClicked?.Invoke();
        CloseUI(true);
    }
}
