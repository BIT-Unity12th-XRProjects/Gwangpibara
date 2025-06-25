using System;
using UnityEngine;

public class ARItemObject : ARObject
{
    private ItemData _itemData;
    private bool _canGetting = false;
    private bool _isCallGet = false;

    public override void TakeRayHit()
    {
        Debug.Log("Camera Hit");
    }

    public override void TakeClick()
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
}
