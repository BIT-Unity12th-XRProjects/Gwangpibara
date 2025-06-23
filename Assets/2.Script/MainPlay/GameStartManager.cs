using System;
using System.Collections;
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
    [SerializeField] private ARPlayTrackingManager _trackingPrefab;
    private int _selectThemNumber;
    private const int INVALID_NUMBER = -1;
    private void Start()
    {
        _selectThemNumber = INVALID_NUMBER;
        _mainController = FindAnyObjectByType<MainController>();
        SetThemeList();
        SelectTheme(1); //테스트로 1번 문제 지정
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

        //MapMaker에서 선택한 주제의 맵 데이터로 맵 세팅 3초 UI GameUI 으로 전환 ->
        //MainController의 SetStep에 주제의 1번 문제로 단계 세팅
        //
        StartCoroutine(CoLoadGame());
    }

    IEnumerator CoLoadGame()
    {
        Debug.Log("맵 만들기 시작");
        UIManager.Instance.RequestOpenUI<OriginSetUI>(); // 원점 잡기용 UI 켜고
        ARPlayTrackingManager trackingManager = Instantiate(_trackingPrefab);
        trackingManager.OnTrackingEnd += SetOriginPos;
        while (true)
        {
            Debug.Log("원점 잡기 대기중");
            yield return null;
            //트래킹 잡힐떄까지
            if(isFind == true)
            {
                Debug.Log("원점 잡아서 나가기");
                break;
            }
        }
        yield return StartCoroutine(MapGenerator.Instance.C_CallGenerator(1)); //맵 만드는 작업을 호출하고 맵 완성을 기다릴것
        _mainController.StartGame(10101); //로드할 단계로 게임 시작 호출, 테스트값  10101
        UIManager.Instance.RequestOpenUI<GameUI>();
    }

    private Vector3 originPosition;
    private bool isFind = false;
    private void SetOriginPos(Vector3 position)
    {
        Debug.Log(position + "원점으로 잡혔다.");
        originPosition = position;
        isFind = true;
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

