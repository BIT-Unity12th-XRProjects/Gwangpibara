using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIData : BaseUIData
{
    public Action onInventoryBtnClicked = null;
    public Action onARBtnClicked = null;
    public Action onBeforeBtnClicked = null;
    public Action onNextBtnClicked = null;
    public Action onSubmitBtnClicked = null;

    public string QuestText;
    public string AnswerText;
}

public class GameUI : BaseUI
{
    public TextMeshProUGUI QuestText;
    public TMP_InputField AnswerInputField;

    // public Button InventoryBtn;
    // public Button ARModeBtn;
    // public Button BeforeBtn;
    // public Button NextBtn;
    // public Button SubmitBtn;

    private Action m_onInventoryBtnClicked;
    private Action m_onARBtnClicked;
    private Action m_onBeforeBtnClicked;
    private Action m_onNextBtnClicked;
    private Action m_onSubmitBtnClicked;

    private GameUIData m_gameUIData;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_gameUIData = uiData as GameUIData;

        m_onInventoryBtnClicked = m_gameUIData.onInventoryBtnClicked;
        m_onARBtnClicked = m_gameUIData.onARBtnClicked;
        m_onBeforeBtnClicked = m_gameUIData.onBeforeBtnClicked;
        m_onNextBtnClicked = m_gameUIData.onNextBtnClicked;
        m_onSubmitBtnClicked = m_gameUIData.onSubmitBtnClicked;

    }

    public void OnClickedInventoryBtn()
    {
        m_onInventoryBtnClicked?.Invoke();
    }
    public void OnClickedARBtn()
    {
        m_onARBtnClicked?.Invoke();
    }
    public void OnClickedBeforeBtn()
    {
        m_onBeforeBtnClicked?.Invoke();
    }
    public void OnClickedNextBtn()
    {
        m_onNextBtnClicked?.Invoke();
    }
    public void OnClickedConfirmBtn()
    {
        m_onSubmitBtnClicked?.Invoke();
    }
}
