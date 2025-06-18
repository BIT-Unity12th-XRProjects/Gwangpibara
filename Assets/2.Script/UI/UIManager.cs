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

    private void Start()
    {
        OpenUI<StartUI>(new StartUIData());
    }

    public void OpenUI<T>(BaseUIData uiData = null)
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

        var siblingIndex = UICanvasTrs.childCount;
        ui.Init(UICanvasTrs);
        ui.transform.SetSiblingIndex(siblingIndex);
        ui.gameObject.SetActive(true);
        ui.SetInfo(uiData);
        ui.ShowUI();

        if (_openUI != null)
        {
            CloseUI(_openUI);
        }

        _openUI = ui;

    }

    private BaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        Type uiType = typeof(T);
        BaseUI ui = null;
        isAlreadyOpen = false;
        
        if(_openUI != null && _openUI.GetType() == uiType)
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

    public void CloseUI(BaseUI ui)
    {
        if (ui == null) return;
        Type uiType = ui.GetType();

        ui.gameObject.SetActive(false);
        _closedUIPool[uiType] = ui.gameObject;
        ui.transform.SetParent(CloseUITrs);
    }

}