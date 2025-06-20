using System;
using UnityEngine;

public class ARMarkerObject : MonoBehaviour, IDetect
{
    private GameMarkerData _markerData;

    // �ʱ�ȭ�� �ܼ��ϱ� ���� ����
    private bool _initialized = false;

    // �������� �ѹ� �����Ǹ� ���Ŀ� ���� ���� �ʰ� �ϱ����� ����
    private bool _isCreate = false;

    void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] ARMarkerObject �� �ʱ�ȭ���� �ʾҽ��ϴ�. �ݵ�� Setting(marker) �� ȣ���ϼ���.", this);
        }
        Debug.Log($"_markId : {_markerData.markId}, _markerType : {_markerData.markerType}");

    }

    public void TakeRayHit()
    {
        MarkerType thisMarkerType = _markerData.markerType;

        switch(thisMarkerType)
        {
            case MarkerType.DropItem:
                CreateItemPefab();
                break;
            case MarkerType.Clue:
                break;
            case MarkerType.SelfClue:
                break;
            case MarkerType.Decoration:
                break;
            case MarkerType.Trap:
                break;
            default:
                break;
        }
    }

    private void CreateItemPefab()
    {
        if (_isCreate == false)
        {
            ItemData item = MasterDataManager.Instance.GetMasterItemData(_markerData.dropItemId);

            // �������� �ʴ� ������ ID�� ������ NULL ��ȯ�Ǿ �Լ� ��ŵ
            if(item == null)
            {
                return;
            }

            GameObject gameObject = Instantiate(item.cachedObject, transform.position + Vector3.up, Quaternion.identity);
            
            gameObject.AddComponent<ARItemObject>().Setting(item);

            _isCreate = true;
        }
    }

    public void TakeClick()
    {
        Debug.Log("Click Obj");
    }

    public void Setting(GameMarkerData markerData)
    {
        _markerData = markerData;

        _initialized = true;
    }
}
