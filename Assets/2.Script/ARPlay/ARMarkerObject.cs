using System;
using UnityEngine;

public class ARMarkerObject : MonoBehaviour, IDetect
{
    private GameMarkerData _markerData;

    // 아이템이 한번 생성되면 이후에 생성 되지 않게 하기위한 변수
    private bool _isCreate = false;
    private bool _isRenderOn = false;

    private void Start()
    {

        Debug.Log($"_markId : {_markerData.markId}, _markerType : {_markerData.markerType}");

        if (CheckWallType())
        {
            return;
        }

        OnCloseTypeSetting();
    }

    private void OnCloseTypeSetting()
    {
        MarkerType thisMarkerType = _markerData.markerType;
    }
    public  void TakeRayHit()
    {
        // TODO : 카메라 정면 레일 맞았을때 할일
    }

    private void CreateItemPefab()
    {
        if (_isCreate == false)
        {
            ItemData item = MasterDataManager.Instance.GetMasterItemData(_markerData.dropItemId);

            // 존재하지 않는 아이템 ID를 받으면 NULL 반환되어서 함수 스킵
            if (item == null)
            {
                return;
            }
            
            ItemData needItem = MasterDataManager.Instance.GetMasterItemData(_markerData.needItemId);

            if (needItem != null)
            {
                int amount = MainController.Instance.GetItemInventory().GetItemAmount(_markerData.needItemId);

                if (amount < 1)
                {
                    return;
                }
            }

            GameObject gameObject = Instantiate(item.cachedObject, transform.position + Vector3.up, Quaternion.identity);

            gameObject.AddComponent<ARItemObject>().Setting(item);

            _isCreate = true;
        }
    }

    public void Setting(GameMarkerData markerData)
    {
        _markerData = markerData;
    }

    public void TakeCloseOverlap()
    {
        if (CheckWallType())
        {
            return;
        }

        CheckSpawnTypes();
    }

    private void OnSurprize()
    {
        if (_isRenderOn == false)
        {

            CheckTypes();

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
    }

    private void CheckSpawnTypes()
    {
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

    public void NotTakeDetect()
    {
        if (CheckWallType())
        {
            return;
        }
    }

    public void TakeClick()
    {
        CheckTypes();
    }

    private bool CheckWallType()
    {
        if (_markerData.markerType == MarkerType.Wall)
        {
            Material occlusionMat = Resources.Load<Material>("OcclusionMaterial1");
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material = occlusionMat;
            return true;
        }
        return false;
    }
}
