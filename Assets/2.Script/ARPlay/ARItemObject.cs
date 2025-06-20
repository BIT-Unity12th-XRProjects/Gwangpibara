using UnityEngine;

public class ARItemObject : MonoBehaviour, IDetect
{
    private ItemData _thisItemData;

    // 초기화를 단속하기 위한 변수
    private bool _initialized = false;

    void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] ARMarkerObject 가 초기화되지 않았습니다. 반드시 Setting(marker) 를 호출하세요.", this);
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
