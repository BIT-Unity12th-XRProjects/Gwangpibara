using System;
using UnityEngine;

public class ARItemObject : MonoBehaviour, IDetect
{
    private ItemData _itemData;
    private bool _canGetting = false;
    private bool _isCallGet = false;

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
    }

    public void TakeCloseOverlap()
    {
        //TODO : �������� Ž�� �Ǿ����� ����
    }

    public void NotTakeDetect()
    {
        //TODO : �������� Ž�� ������ ������� ����
    }
}
