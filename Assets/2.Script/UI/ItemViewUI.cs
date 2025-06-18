using System;
using UnityEngine;
using UnityEngine.UI;


public class ItemViewUI : BaseUI
{
    public Button ExitButton = null;

    private ItemViewData _viewItemData;
    private Button m_onExitBtnClicked = null;
    [SerializeField] private ItemViewer _itemViewer;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        _viewItemData = uiData as ItemViewData;
        m_onExitBtnClicked.onClick.AddListener(OnClickedExitButton);
        _itemViewer.ViewItem(_viewItemData);
    }

    public void OnClickedExitButton()
    {

    }
}
