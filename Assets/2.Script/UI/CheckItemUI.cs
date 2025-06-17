using System;
using UnityEngine;
using UnityEngine.UI;

public class CheckItemUIData : BaseUIData
{
    public Action onExitBtnClicked;
}

public class CheckItemUI : BaseUI
{
    public Button ExitButton = null;

    private CheckItemUIData m_checkItemUIData;
    private Action m_onExitBtnClicked = null;
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_checkItemUIData = uiData as CheckItemUIData;

        m_onExitBtnClicked = m_checkItemUIData.onExitBtnClicked;
    }

    public void OnClickedExitButton()
    {
        m_onExitBtnClicked?.Invoke();
        CloseUI(true);
    }
}
