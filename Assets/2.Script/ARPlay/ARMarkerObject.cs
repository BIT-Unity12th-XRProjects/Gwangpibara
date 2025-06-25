using System;
using UnityEngine;

public class ARMarkerObject : ARObject
{
    private GameMarkerData _markerData;

    // 아이템이 한번 생성되면 이후에 생성 되지 않게 하기위한 변수
    private bool _isCreate = false;
    private bool _isRenderOn = false;

    private int _WallLayerNum = 6;

    protected override void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] ARMarkerObject 가 초기화되지 않았습니다. 반드시 Setting(marker) 를 호출하세요.", this);
        }

        Debug.Log($"_markId : {_markerData.markId}, _markerType : {_markerData.markerType}");

        if (CheckWallType())
        {
            return;
        }

        OnCloseTypeSetting();

        _renderer.enabled = false;
    }

    private void OnCloseTypeSetting()
    {
        MarkerType thisMarkerType = _markerData.markerType;

        if (_markerData.markerSpawnType == MarkerSpawnType.OnClose)
        {
            _renderer.material.color = Color.yellow;
        }
    }
    public override void TakeRayHit()
    {
        Debug.Log("Ray Hit");
        CheckTypes();
    }

    private void CreateItemPefab()
    {
        if (_isCreate == false)
        {
            ItemData item = MasterDataManager.Instance.GetMasterItemData(_markerData.dropItemId);

            // 존재하지 않는 아이템 ID를 받으면 NULL 반환되어서 함수 스킵
            if (item == null)
            {
                Debug.Log("아이템이 없음");
                return;
            }

            GameObject gameObject = Instantiate(item.cachedObject, transform.position + Vector3.up, Quaternion.identity);

            gameObject.AddComponent<ARItemObject>().Setting(item);

            _isCreate = true;
        }
    }

    public void Setting(GameMarkerData markerData)
    {
        _markerData = markerData;

        _initialized = true;
    }

    public override void TakeCloseOverlap()
    {
        if (CheckWallType())
        {
            return;
        }

        base.TakeCloseOverlap();

        CheckTypes();
    }

    private void OnSurprize()
    {
        if (_isRenderOn == false)
        {
            _renderer.material.color = Color.blue;

            _isRenderOn = true;
        }
    }

    private void CheckTypes()
    {
        MarkerType thisMarkerType = _markerData.markerType;

        switch (thisMarkerType)
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
            case MarkerType.Wall:
                break;
            default:
                break;
        }

        MarkerSpawnType thisMarkerSpawnType = _markerData.markerSpawnType;

        switch (thisMarkerSpawnType)
        {
            case MarkerSpawnType.Base:
                break;
            case MarkerSpawnType.OnClose:
                OnSurprize();
                break;
            default:
                break;
        }
    }

    public override void NotTakeDetect()
    {
        if (CheckWallType())
        {
            return;
        }

        base.NotTakeDetect();
    }

    public override void TakeClick()
    {
        Debug.Log("Click");
    }

    private bool CheckWallType()
    {
        if (_markerData.markerType == MarkerType.Wall)
        {
            gameObject.layer = _WallLayerNum;
            return true;
        }
        return false;
    }
}
