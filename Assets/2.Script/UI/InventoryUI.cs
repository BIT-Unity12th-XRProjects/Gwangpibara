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


    public override void SetInfo(BaseUIData uiData)
    {

    }

    public void OnClickedExitButton()
    {
        m_onExitBtnClicked?.Invoke();
        CloseUI(true);
    }
}
