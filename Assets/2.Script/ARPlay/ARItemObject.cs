using UnityEngine;

public class ARItemObject : MonoBehaviour, IDetect
{
    private ItemData _itemData;

    // �ʱ�ȭ�� �ܼ��ϱ� ���� ����
    private bool _initialized = false;

    void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] ARMarkerObject �� �ʱ�ȭ���� �ʾҽ��ϴ�. �ݵ�� Setting(marker) �� ȣ���ϼ���.", this);
        }

        Debug.Log($"_itemData.ID : {_itemData.ID}, _itemData.Name : {_itemData.Name}");
    }

    public void TakeRayHit()
    {
        Debug.Log("Camera Hit");
    }

    public void TakeClick()
    {
        Debug.Log("Click!");
    }

    public void Setting(ItemData itemData)
    {
        _itemData = itemData;
        _itemData.cachedObject = gameObject;

        _initialized = true;
    }
}
