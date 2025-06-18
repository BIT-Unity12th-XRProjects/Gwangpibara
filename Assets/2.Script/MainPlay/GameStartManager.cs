using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{

    /// <summary>
    /// 게임 시작을 위한 관리
    /// 1. 주제 리스트를 보여주기
    /// 2. 플레이어가 선택한 주제로 게임 시작
    /// </summary>
    /// 

    [SerializeField] private List<ThemeData> _themeList;
    [SerializeField] private MainController _mainController;
    private int _selectThemNumber;
    private const int INVALID_NUMBER = -1;
    private void Start()
    {
        _selectThemNumber = INVALID_NUMBER;
        SetThemeList();
    }

    public void SelectTheme(int themeNumber)
    {
        _selectThemNumber = themeNumber;
    }

    public void StartGame()
    {
        if(_selectThemNumber == INVALID_NUMBER)
        {
            return;
        }

        //MapMaker에서 선택한 주제의 맵 데이터로 맵 세팅
        //MainController의 SetStep에 주제의 1번 문제로 단계 세팅
        //UI GameUI 으로 전환

    }

    private void SetThemeList()
    {
        _themeList = new List<ThemeData>();
        foreach(KeyValuePair<int, ThemeData> theme in MasterDataManager.Instance.GetMasterThemeDataDic())
        {
            _themeList.Add(new ThemeData(theme.Value));
        }
    }
}

