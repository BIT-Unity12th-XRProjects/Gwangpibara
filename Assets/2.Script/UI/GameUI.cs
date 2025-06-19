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

    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _aRButton;
    [SerializeField] private Button _beforeButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _submitButton;

    private GameUIData m_gameUIData;

    protected override void Awake()
    {
        base.Awake();
        _inventoryButton.onClick.AddListener(OnClickedInventoryButton);
        _aRButton.onClick.AddListener(OnClickedARButton);
        _beforeButton.onClick.AddListener(OnClickedBeforeButton);
        _nextButton.onClick.AddListener(OnClickedNextButton);
        _submitButton.onClick.AddListener(OnClickSubmitButton);
    }

    public override void SetInfo(BaseUIData uiData)
    {
        //게임 컨틀로러의 게임데이터 가져와서 세팅하기, 구독하기
        


    }

    private void OnClickedInventoryButton()
    {
        UIManager.Instance.OpenUI<InventoryUI>();
    }

    private void OnClickedARButton()
    {

    }

    private void OnClickedBeforeButton()
    {

    }

    private void OnClickedNextButton()
    {

    }

    private void OnClickSubmitButton()
    {

    }
}
