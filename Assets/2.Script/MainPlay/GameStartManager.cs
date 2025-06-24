using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

        //원점 잡기
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

        MapDownload downloader = new();
        downloader.DownloadMapDate("테스트로컬", 1234);
        float escapeTime = 2f;
        float curTime = 0f;
        while (curTime < escapeTime)
        {
            if(downloader.GameMakerList != null)
            {
                //다운 받았으면 와일문알아서 나감
                break;
            }
            curTime += Time.deltaTime;
            yield return null;
        }

        if(downloader.GameMakerList != null && downloader.GameMakerList.Count > 0)
        {
            yield return MasterDataManager.Instance.DownLoadMap(downloader.GameMakerList, 1234);
            _selectThemNumber = 1234;
            Debug.Log("다운받은게 있어서 1234 맵으로");
        }
        else
        {
            Debug.Log("다운 받은게 없음");
        }

        //맵 생성
        mapParent = new GameObject("MapParent").transform;
        mapParent.position = Vector3.zero; //원점 0,0,0 
        yield return StartCoroutine(MapGenerator.Instance.C_CallGenerator(_selectThemNumber, mapParent)); //맵 만드는 작업을 호출하고 맵 완성을 기다릴것

        //원점 조정
        mapParent.position = originPosition; //맵 부모 좌표를 이미지트래킹으로 잡은 원점으로 이동
        mapParent.rotation = Quaternion.Euler(-90f, 0f, 0f);
        mapParent.rotation = originQuaternion;
        
        //메인 컨트롤러 시작
        _mainController.StartGame(10101); //로드할 단계로 게임 시작 호출, 테스트값  10101
        UIManager.Instance.RequestOpenUI<GameUI>();
    }

    private Transform mapParent;
    private Vector3 originPosition;
    private Quaternion originQuaternion;
    private bool isFind = false;
    private void SetOriginPos(Vector3 position, Quaternion quaternion)
    {
        Debug.Log(position + "원점으로 잡혔다.");
        originPosition = position;
        originQuaternion = quaternion;
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

