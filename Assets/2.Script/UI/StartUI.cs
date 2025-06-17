using System;
using TMPro;
using UnityEngine;

public class StartUIData : BaseUIData
{
    public string DescriptionText;

    public Action onStartGameBtnClicked;
}

public class StartUI : BaseUI
{
    public TextMeshProUGUI DescText;

    private StartUIData m_startUIData;
    private Action m_onStartGameBtnclicked;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_startUIData = uiData as StartUIData;

        DescText.text = m_startUIData.DescriptionText;
        m_onStartGameBtnclicked = m_startUIData.onStartGameBtnClicked;
    }

    public void OnClickedStartBtn()
    {
        m_onStartGameBtnclicked?.Invoke();
    }
}
