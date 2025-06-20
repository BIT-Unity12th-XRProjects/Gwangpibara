using System;
using UnityEngine;

public class ARMarkerObject : MonoBehaviour
{
    private GameMarkerData _markerData;

    // 초기화를 단속하기 위한 변수
    private bool _initialized = false;

    void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] ARMarkerObject 가 초기화되지 않았습니다. 반드시 Setting(marker) 를 호출하세요.", this);
        }

        Debug.Log($"_markId : {_markerData.markId}, _markerType : {_markerData.markerSapwnType}");
    }

    public void TakeRayHit()
    {
        Debug.Log("Camera Hit");
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
