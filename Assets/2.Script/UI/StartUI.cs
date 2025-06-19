using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUIData : BaseUIData
{
    public string DescriptionText;

    public Action onStartGameBtnClicked;
}

public class StartUI : BaseUI
{
    public TextMeshProUGUI DescText;

    private StartUIData _startUIData;
    private GameStartManager _startManager;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startManager = FindAnyObjectByType<GameStartManager>();
        _startButton.onClick.AddListener(OnClickedStartBtn);
    }

    public override void SetInfo(BaseUIData uIData)
    {
       //��ŸƮ �Ŵ����� �����͸� ���� �ͼ� �Ұ� 

    }

    public void OnClickedStartBtn()
    {
        _startManager.StartGame();
    }
}
