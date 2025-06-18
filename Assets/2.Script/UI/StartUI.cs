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

    private StartUIData m_startUIData;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startButton.onClick.AddListener(OnClickedStartBtn);
    }

    public override void SetInfo(BaseUIData uIData)
    {
       //��ŸƮ �Ŵ����� �����͸� ���� �ͼ� �Ұ� 

    }

    public void OnClickedStartBtn()
    {
        UIManager.Instance.OpenUI<GameUI>();
    }
}
