using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIData : BaseUIData
{
    public string QuestText;
    public string HintText;
    public string AnswerText;

    public void SetData(StepData stepData)
    {
        QuestText = stepData.PrintText;
        HintText = stepData.Hint;
    }
}

public class GameUI : BaseUI
{
    public TextMeshProUGUI QuestText;
    public TextMeshProUGUI HintText;
    public TMP_InputField AnswerInputField;

    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _aRButton;
    [SerializeField] private Button _beforeButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _hintButton;
    [SerializeField] private Button _submitButton;

    private MainController _mainController;

    protected override void Awake()
    {
        base.Awake();
        _inventoryButton.onClick.AddListener(OnClickedInventoryButton);
        _aRButton.onClick.AddListener(OnClickedARButton);
        _beforeButton.onClick.AddListener(OnClickedBeforeButton);
        _nextButton.onClick.AddListener(OnClickedNextButton);
        _submitButton.onClick.AddListener(OnClickSubmitButton);
        _hintButton.onClick.AddListener(OnClickHintButton);
    }

    private void Start()
    {
        _mainController = FindAnyObjectByType<MainController>();
        SetInfo(_mainController.GetGameUIData()); //처음 세팅할땐 가져와서
        _mainController.onChangeStepData += SetInfo; //이후 변화에 따라서
        _mainController.onShowHint += ShowHint;
    }

    public override void SetInfo(BaseUIData uiData)
    {
        if(uiData == null)
        {
            return;
        }
        //게임 컨틀로러의 게임데이터 가져와서 세팅하기, 구독하기
        QuestText.text = ((GameUIData)uiData).QuestText;
        HintText.text = "";
    }

    private void ShowHint(GameUIData uiData)
    {
        HintText.text = uiData.HintText;
    }

    private void OnClickedInventoryButton()
    {
        UIManager.Instance.RequestOpenUI<InventoryUI>();
    }

    private void OnClickedARButton()
    {
        UIManager.Instance.RequestOpenUI<ARPlayUI>();
    }

    private void OnClickedBeforeButton()
    {
        _mainController.ClickBeforeButton();
    }

    private void OnClickedNextButton()
    {
        _mainController.ClickNextButton();
    }

    private void OnClickSubmitButton()
    {
        _mainController.SubmitAnswer(AnswerInputField.text);
    }

    private void OnClickHintButton()
    {
        _mainController.PleaseHint();
    }
}
