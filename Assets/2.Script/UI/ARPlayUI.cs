using UnityEngine;
using UnityEngine.UI;

public class ARPlayUI : BaseUI
{
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _aRButton;

    protected override void Awake()
    {
        base.Awake();
        _inventoryButton.onClick.AddListener(OnClickedInventoryButton);
        _aRButton.onClick.AddListener(OnClickedARButton);
    }

    private void OnClickedInventoryButton()
    {
        UIManager.Instance.RequestOpenUI<InventoryUI>();
    }

    private void OnClickedARButton()
    {
        UIManager.Instance.RequestOpenUI<GameUI>();
    }
}

