using System;
using UnityEngine;

public class ARItemObject : MonoBehaviour, IDetect
{
    private ItemData _itemData;
    private bool _canGetting = false;
    private bool _isCallGet = false;

    // �ʱ�ȭ�� �ܼ��ϱ� ���� ����
    private bool _initialized = false;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] ARMarkerObject �� �ʱ�ȭ���� �ʾҽ��ϴ�. �ݵ�� Setting(marker) �� ȣ���ϼ���.", this);
        }

        Debug.Log($"_itemData.ID : {_itemData.ID}, _itemData.Name : {_itemData.Name}");

        _renderer.enabled = false;
    }

    public void TakeRayHit()
    {
        Debug.Log("Camera Hit");
    }

    public void TakeClick()
    {
        Debug.Log("Click!");

        GetItem();
    }

    private void GetItem()
    {
        if (_isCallGet)
        {
            return;
        }

        _isCallGet = true;

        _canGetting = MainController.Instance.AcquireItem(_itemData);

        if (_canGetting)
        {
            Destroy(gameObject, 0.5f);
        }
        else
        {
            _isCallGet = false;
        }
    }

    public void Setting(ItemData itemData)
    {
        _itemData = itemData;

        _initialized = true;
    }

    public void TakeCloseOverlap()
    {
        Debug.Log("OverLap");

        _renderer.enabled = true;
    }

    public void NotTakeDetect()
    {
        _renderer.enabled = false;
    }
}
