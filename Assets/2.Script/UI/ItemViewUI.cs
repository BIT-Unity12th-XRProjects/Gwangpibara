using System;
using UnityEngine;
using UnityEngine.UI;


public class ItemViewUI : BaseUI
{
    private ItemViewData _viewItemData;
    [SerializeField] private Button _exitButton = null;
    [SerializeField] private ItemViewer _itemViewer;

    protected override void Awake()
    {
        base.Awake();
        _exitButton.onClick.AddListener(OnClickedExitButton);
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        _viewItemData = uiData as ItemViewData;
        _itemViewer.ViewItem(_viewItemData);
    }

    public void OnClickedExitButton()
    {

    }
}
