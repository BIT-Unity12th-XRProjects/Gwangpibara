using System;
using UnityEngine;

public class ARMarkerObject : MonoBehaviour, IDetect
{
    private GameMarkerData _markerData;

    // 초기화를 단속하기 위한 변수
    private bool _initialized = false;

    // 아이템이 한번 생성되면 이후에 생성 되지 않게 하기위한 변수
    private bool _isCreate = false;

    void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] ARMarkerObject 가 초기화되지 않았습니다. 반드시 Setting(marker) 를 호출하세요.", this);
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
            ItemData item = MasterDataManager.Instance.GetMasterItemData(10101);

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
        _markerData.markerGameObject = gameObject;

        _initialized = true;
    }
}
