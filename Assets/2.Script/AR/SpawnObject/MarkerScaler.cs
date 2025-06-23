using UnityEngine;

public class MarkerScaler : MonoBehaviour
{
    private GameObject selectedMarker;
    [SerializeField] private GameObject _markerUpdateDataUI;
    
    [Header("Scale")]
    public bool isScaleMode = false;
    [SerializeField] private GameObject _markerScaleUI;

    private readonly Vector3 scaleDelta = new Vector3(0.02f, 0.02f, 0.02f);

    /// <summary>
    /// 선택한 마커 
    /// </summary>
    /// <param name="marker">선택 마커</param>
    public void SetSelectedMarker(GameObject marker)
    {
        selectedMarker = marker;
    }

    /// <summary>
    /// 스케일 모드 On
    /// </summary>
    public void OnScaleModeButtonPressed()
    {
        isScaleMode = true;
        _markerUpdateDataUI.SetActive(false);
        _markerScaleUI.SetActive(true);
    }

    /// <summary>
    /// 스케일 모드 Off
    /// </summary>
    public void OffScaleModeButtonPressed()
    {
        isScaleMode = false;
        _markerUpdateDataUI.SetActive(true);
        HideScaleUI();
    }

    /// <summary>
    /// 선택한 마커 스케일 UI 표시
    /// </summary>
    /// <param name="target">선택한 마커</param>
    public void ShowScaleUI(GameObject target)
    {
        selectedMarker = target;
        _markerScaleUI.SetActive(true);
    }

    /// <summary>
    /// 스케일 UI 숨기기
    /// </summary>
    private void HideScaleUI()
    {
        _markerScaleUI.SetActive(false);
    }

    /// <summary>
    /// 스케일 적용 함수
    /// </summary>
    /// <param name="delta">마커의 Scale 값</param>
    public void ScaleSelectedMarker(Vector3 delta)
    {
        if (selectedMarker == null) return;

        Vector3 newScale = selectedMarker.transform.localScale + delta;

        // 최소 스케일 제한 (0.1 이상)
        if (newScale.x < 0.05f || newScale.y < 0.1f || newScale.z < 0.05f)
            return;

        selectedMarker.transform.localScale = newScale;
    }

    /// <summary>
    /// 각각의 버튼
    /// </summary>
    public void ScaleUp() => ScaleSelectedMarker(scaleDelta);
    public void ScaleDown() => ScaleSelectedMarker(-scaleDelta);
}