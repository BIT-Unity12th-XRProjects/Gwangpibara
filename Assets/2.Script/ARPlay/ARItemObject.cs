using UnityEngine;

public class ARItemObject : MonoBehaviour, IDetect
{
    private ItemData _thisItemData;

    // �ʱ�ȭ�� �ܼ��ϱ� ���� ����
    private bool _initialized = false;

    void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] ARMarkerObject �� �ʱ�ȭ���� �ʾҽ��ϴ�. �ݵ�� Setting(marker) �� ȣ���ϼ���.", this);
        }

        Debug.Log($"_markId : {_thisItemData.ID}, _markerType : {_thisItemData.Name}");
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
        _thisItemData = itemData;
        _thisItemData.cachedObject = gameObject;

        _initialized = true;
    }
}
