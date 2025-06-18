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

        //�׽�Ʈ�� �½��� - �κ��丮 UI���� �ؾ�����
        SetSlot(null);
    }

    public void SetSlot(ItemData itemData)
    {
        _item = itemData;
        //_itemViewData = new ItemViewData(_item);


        //�׽�Ʈ�� 
        ItemData data = MasterDataManager.Instance.GetMasterItemData(10101);
        _itemViewData = new ItemViewData(data);
    }

    private void OnClickSlot()
    {
        UIManager.Instance.OpenUI<ItemViewUI>(_itemViewData);
    }

}
