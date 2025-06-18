using UnityEngine;
using UnityEngine.UI;


public class ItemViewData : BaseUIData
{
    public int itemId;
    public GameObject itemPrefab;

    public ItemViewData(ItemData itemData)
    {
        itemId = itemData.ID;
        itemPrefab = itemData.cachedObject;
    }
}

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Button _slotButton;
    [SerializeField] private ItemData _item;
    private ItemViewData _itemViewData;

    private void Awake()
    {
        _slotButton.onClick.AddListener(OnClickSlot);

        //테스트로 셋슬랏 - 인벤토리 UI에서 해야할일
        SetSlot(null);
    }

    public void SetSlot(ItemData itemData)
    {
        _item = itemData;
        //_itemViewData = new ItemViewData(_item);


        //테스트용 
        ItemData data = MasterDataManager.Instance.GetMasterItemData(10101);
        _itemViewData = new ItemViewData(data);
    }

    private void OnClickSlot()
    {
        UIManager.Instance.OpenUI<ItemViewUI>(_itemViewData);
    }

}
