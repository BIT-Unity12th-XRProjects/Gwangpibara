using System;
using UnityEngine;
using UnityEngine.UI;


public class ItemViewUI : BaseUI
{
    private ItemViewData _viewItemData;
    [SerializeField] private Button _exitButton;
    [SerializeField] private ItemViewer _itemViewer;
    private Canvas _canvas;

    protected override void Awake()
    {
        base.Awake();
        _exitButton.onClick.AddListener(OnClickedExitButton);
        _canvas = GetComponent<Canvas>();
        _canvas.planeDistance = 2;
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        _viewItemData = uiData as ItemViewData;
        _itemViewer.ViewItem(_viewItemData);
    }

    public void OnClickedExitButton()
    {
        _itemViewer.DestroyItem();
        CloseUI(true);
    }
}
