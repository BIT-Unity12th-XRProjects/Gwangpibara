using System;
using UnityEngine;

public class BaseUIData
{
    //동일한 UI여도 상황에 따라 다른 걸 띄워야 할 때가 있기 때문에
    public Action OnShow;
    public Action OnClose;

    public bool isAnimPlay = false;
}

public enum UIType
{
    None, GameStart, Play, ArMode, Inventory, ItemViewer, OriginSet
}

public class BaseUI : MonoBehaviour
{
    public bool m_isAnimPlay;
    public UIType UIType;

    private Action m_OnShow;
    private Action m_OnClose;

    protected virtual void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    public virtual void Init(Transform anchor)
    {

        m_OnShow = null;
        m_OnClose = null;

        transform.SetParent(anchor);

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector3.zero;
        rectTransform.offsetMax = Vector3.zero;

    }


    public virtual void SetInfo(BaseUIData uiData = null)
    {
        if(uiData == null)
        {
            return;
        }

        m_isAnimPlay = uiData.isAnimPlay;

        m_OnShow = uiData.OnShow;
        m_OnClose = uiData.OnClose;
    }

    public virtual void ShowUI()
    {
        m_OnShow?.Invoke();
        m_OnShow = null;
    }

    public virtual void CloseUI(bool isCloseAll = false) 
    {
        if(!isCloseAll) //씬전환등 열려있는 모든 화면을 닫아야 할 때 true로 넘겨서 필요한 처리 다 무시하고 화면만 닫아주기 위해
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;
        UIManager.Instance.RequestCloseUI(this);
    }

    //닫기버튼 눌렀을 때 함수 
    public virtual void OnClickCloseButton()
    {
        CloseUI();
    }
}

