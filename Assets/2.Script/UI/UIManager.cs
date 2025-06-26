using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    public Transform UICanvasTrs; //UI화면을 랜더링할 컨버스
    public Transform CloseUITrs; //닫을 때 비활성화 할 

    //열려있는, 닫혀있는 ui pool 나눠서 관리
    private BaseUI _openUI;
    private Dictionary<Type, GameObject> _closedUIPool = new Dictionary<Type, GameObject>();

    public override void Awake()
    {
        base.Awake();
        // UICanvas용 빈 오브젝트 생성
        if (UICanvasTrs == null)
        {
            GameObject uiCanvasObj = new GameObject("UICanvasRoot");
            UICanvasTrs = uiCanvasObj.transform;
        }

        // CloseUI용 빈 오브젝트 생성
        if (CloseUITrs == null)
        {
            GameObject closeUIObj = new GameObject("CloseUIRoot");
            CloseUITrs = closeUIObj.transform;
        }


    }


    private void Update()
    {
        //임시 키보드로 취소키 입력받기
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            OnClickCancle();
        }
    }

    /// <summary>
    /// UI 열기요청
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiData"></param>
    public void RequestOpenUI<T>(BaseUIData uiData = null)
    {
        Type uiType = typeof(T);
        bool isAlreadyOpen = false;
        var ui = GetUI<T>(out isAlreadyOpen);

        if (isAlreadyOpen == true)
        {
            return;
        }

        if (ui == null)
        {
            //Logger.LogError($"{uiType} prefab doesn't exist in Resources");
            return;
        }

        if(_openUI != null)
        {
            CloseUI(_openUI);
        }
        OpenUI(ui, uiData);

    }

    private void OpenUI(BaseUI ui, BaseUIData uiData = null)
    {
        ui.Init(UICanvasTrs);
        ui.gameObject.SetActive(true);
        ui.SetInfo(uiData);
        ui.ShowUI();
        _openUI = ui;
    }

    private BaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        Type uiType = typeof(T);
        BaseUI ui = null;
        isAlreadyOpen = false;

        if (_openUI != null && _openUI.GetType() == uiType)
        {
            isAlreadyOpen = true;
        }
        else if (_closedUIPool.ContainsKey(uiType) == true)
        {
            ui = _closedUIPool[uiType].GetComponent<BaseUI>();
            _closedUIPool.Remove(uiType);
        }
        else
        {
            //모든 UI들이 미리 씬에 세팅되어 있지 않고, 동적으로 처리
            //프리팹의 이름이 uiClass 이름과 동일해야 함 
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject;
            ui = uiObj.GetComponent<BaseUI>();
        }

        return ui;
    }


    /// <summary>
    /// 휴대폰 취소 버튼 눌렸을 때
    /// </summary>
    public void OnClickCancle()
    {
        if (_openUI == null)
        {
            return;
        }

        RequestCloseUI(_openUI);


    }

    /// <summary>
    /// ui 닫으라고 호출 받았을 떄
    /// </summary>
    /// <param name="ui"></param>
    public void RequestCloseUI(BaseUI ui)
    {
        if (ui == null) return;
     
        switch (_openUI.UIType)
        {
            case UIType.GameStart:
                //게임종료하기 팝업
                break;
            case UIType.Play:
                //선택화면 돌아가기 팝업
                break;
            case UIType.ArMode:
            case UIType.Inventory:
            case UIType.ItemViewer:
                RequestOpenUI<GameUI>();
                break;
            case UIType.OriginSet:
                RequestOpenUI<StartUI>();
                break;
        }


    }

    private void CloseUI(BaseUI ui)
    {
        Type uiType = ui.GetType();
        ui.gameObject.SetActive(false);
        _closedUIPool[uiType] = ui.gameObject;
        ui.transform.SetParent(CloseUITrs);
    }

  
}