using UnityEngine;

public class MarkerRotator : MonoBehaviour
{
    private GameObject selectedMarker;
    [SerializeField] private GameObject _markerUpdateDataUI;
    
    [Header("Rotation")]
    public bool isRotateMode = false;
    [SerializeField] private GameObject _markerRotateUI;
    
    // 선택한 마커 반환
    public void SetSelectedMarker(GameObject marker)
    {
        selectedMarker = marker;
    }
    
    // 회전 모드 ON
    public void OnRotateModeButtonPressed()
    {
        isRotateMode = true;
        _markerUpdateDataUI.SetActive(false);
        _markerRotateUI.SetActive(true);
    }

    // 회전 모드 OFF
    public void OffRotateModeButtonPressed()
    {
        isRotateMode = false;
        _markerUpdateDataUI.SetActive(true);
        HideRotateUI();
    }

    // 선택한 오브젝트 회전 UI 보여주기
    public void ShowRotateUI(GameObject target)
    {
        selectedMarker = target;
        _markerRotateUI.SetActive(true);
    }

    // 회전 UI 숨기기
    private void HideRotateUI()
    {
        _markerRotateUI.SetActive(false);
    }

    // 회전 적용 함수
    public void RotateSelectedMarker(Vector3 eulerAngles)
    {
        if (selectedMarker == null) return;

        float rotateAmount = 5f;
        selectedMarker.transform.Rotate(eulerAngles * rotateAmount, Space.Self);
    }

    // 회전용 버튼 함수 (XYZ)
    public void XRotationPlus() => RotateSelectedMarker(Vector3.right);
    public void XRotationMinus() => RotateSelectedMarker(Vector3.left);
    public void YRotationPlus() => RotateSelectedMarker(Vector3.up);
    public void YRotationMinus() => RotateSelectedMarker(Vector3.down);
    public void ZRotationPlus() => RotateSelectedMarker(Vector3.forward);
    public void ZRotationMinus() => RotateSelectedMarker(Vector3.back);

}
