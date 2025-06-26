using System;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIData : BaseUIData
{
    public string TitleText;
    public string DescriptionText;

    public void SetData(ThemeData themeData)
    {
        TitleText = themeData.title;
        DescriptionText = themeData.description;
    }

    public Action onStartGameBtnClicked;
}

public class StartUI : BaseUI
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescText;

    private StartUIData _startUIData;
    private GameStartManager _startManager;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startManager = FindAnyObjectByType<GameStartManager>();
        _startManager.OnChangeTheme += SetInfo;
        _startButton.onClick.AddListener(OnClickedStartBtn);
    }

    public override void SetInfo(BaseUIData uIData)
    {
        //스타트 매니저의 데이터를 가져 와서 할거 
        SetInfo((StartUIData)uIData);
    }
    private void SetInfo(StartUIData startUIData)
    {
        TitleText.text = startUIData.TitleText;
        DescText.text = startUIData.DescriptionText;
    }

    public void OnClickedStartBtn()
    {
        _startManager.StartGame();
    }

    public void OnClickEnterEditor()
    {
        SceneManager.LoadScene("AREditorScene");
    }
}
