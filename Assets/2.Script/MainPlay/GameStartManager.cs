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
    [SerializeField] private MarkersApiClient _markersApiClient;

    private StartUIData _startUIData;
    public Action<StartUIData> OnChangeTheme;
    private int _selectThemNumber;
    private const int INVALID_NUMBER = -1;

    private IEnumerator Start()
    {
        UIManager.Instance.RequestOpenUI<LoadingUI>();
        //파싱매니저로 구글스프레드에서 파싱
        yield return new GameObject("ParsingManager").AddComponent<ParsingManager>().ParseSheetData();
        //파싱한 데이터로 마스터 데이터 생성
        yield return new GameObject("masterDataManager").AddComponent<MasterDataManager>().SetData();
        //서버 맵 데이터 받기 위한 clientapi 생성
        yield return _markersApiClient = new GameObject("MarkersApi").AddComponent<MarkersApiClient>();
        
        _startUIData = new();
        _selectThemNumber = INVALID_NUMBER;
        _mainController = FindAnyObjectByType<MainController>();
        SetThemeList();
        SelectTheme(1); //테스트로 1번 문제 지정

        UIManager.Instance.RequestOpenUI<StartUI>(_startUIData);
    }

    public void SelectTheme(int themeNumber)
    {
        _selectThemNumber = themeNumber;

        ThemeData themeData = MasterDataManager.Instance.GetMasterThemeData(_selectThemNumber);
        _startUIData.SetData(themeData);
        OnChangeTheme?.Invoke(_startUIData);
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

        PopUpManager.Instance.PopMessege("카피바라를 찍으세요");
        //원점 잡기
        UIManager.Instance.RequestOpenUI<OriginSetUI>(); // 원점 잡기용 UI 켜고
        ARPlayTrackingManager trackingManager = Instantiate(_trackingPrefab);
        trackingManager.OnTrackingEnd += SetOriginPos;
        trackingManager.OnTrackingStart += SetTrackingStart;

        //원점 잡기 대기
        float messageTimer = 0f;

        while (true)
        {
            Debug.Log("원점 잡기 대기중");

            messageTimer += Time.deltaTime;
            if (messageTimer >= 3f)
            {
                if(isTrackingStart == false)
                {
                    PopUpManager.Instance.PopMessege("카피바라를 찍으세요");
                }
                else
                {
                    PopUpManager.Instance.PopMessege("원점을 잡는 중입니다.");
                }

                messageTimer = 0f;
            }

            if (isOriginPosFind == true)
            {
                Debug.Log("원점 잡아서 나가기");
                break;
            }

            yield return null;

        }
        //

        bool isDownloadDone = false;
        //다운로드 팝업을 위한 병렬 코루틴
        StartCoroutine(CoDownloadMessageRoutine(() => isDownloadDone));

        // ▼ 실제 다운로드
        List<GameMarkerData> gameDataList = new();
        yield return StartCoroutine(_markersApiClient.GetAllMarkers(
            markers =>
            {
                Debug.Log("다운 성공");
                DownloadGameMarkData(markers, gameDataList);
                isDownloadDone = true; // 메시지 코루틴을 멈춤
            },
            err =>
            {
                Debug.LogError("다운로드 실패");
                isDownloadDone = true; // 실패 시에도 메시지 코루틴 멈춤
            }
        ));

        if (gameDataList.Count > 0)
        {
            yield return MasterDataManager.Instance.DownLoadMap(gameDataList, 1);
            _selectThemNumber = 1;
            Debug.Log("다운받은게 있어서 1234 맵으로");
        }
        else
        {
            Debug.Log("다운 받은게 없음");
        }

        //

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

    private void DownloadGameMarkData(ServerMarkerData[] serverData, List<GameMarkerData> gameDataList)
    {
        for (int i = 0; i < serverData.Length; i++)
        {
            gameDataList.Add(new GameMarkerData(serverData[i]));
        }
    }

    private IEnumerator CoDownloadMessageRoutine(System.Func<bool> isDoneCheck)
    {
        while (!isDoneCheck())
        {
            PopUpManager.Instance.PopMessege("다운로드 중입니다...");
            yield return new WaitForSeconds(3f);
        }
    }


    private Transform mapParent;
    private Vector3 originPosition;
    private Quaternion originQuaternion;
    private bool isOriginPosFind = false;
    private bool isTrackingStart = false;
    private void SetOriginPos(Vector3 position, Quaternion quaternion)
    {
        PopUpManager.Instance.PopMessege("원점을 잡았습니다.");
        originPosition = position;
        originQuaternion = quaternion;
        isOriginPosFind = true;
    }

    private void SetTrackingStart()
    {
        isTrackingStart = true;
        PopUpManager.Instance.PopMessege("원점을 잡는 중입니다.");
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

