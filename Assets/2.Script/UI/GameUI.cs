using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIData : BaseUIData
{
    public string QuestText;
    public string AnswerText;

    public void SetData(StepData stepData)
    {
        QuestText = stepData.PrintText;
    }
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

    private MainController _mainController;
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

    private void Start()
    {
        _mainController = FindAnyObjectByType<MainController>();
        SetInfo(_mainController.GetGameUIData()); //ó�� �����Ҷ� �����ͼ�
        _mainController.onChangeStepData += SetInfo; //���� ��ȭ�� ����
    }

    public override void SetInfo(BaseUIData uiData)
    {
        if(uiData == null)
        {
            return;
        }
        //���� ��Ʋ�η��� ���ӵ����� �����ͼ� �����ϱ�, �����ϱ�
        QuestText.text = ((GameUIData)uiData).QuestText;
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
}
