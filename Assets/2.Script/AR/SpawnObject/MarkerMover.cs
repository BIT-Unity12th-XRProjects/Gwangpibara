using UnityEngine;
using UnityEngine.UI;

public class MarkerMover : MonoBehaviour
{
    private GameObject selectedMarker;
    [SerializeField] private GameObject _markerUpdateDataUI;
    [Header("Position")]
    public bool isMoveMode = false;
    [SerializeField] private GameObject _markerMoverUI;
    
    // 선택한 마커 반환
    public void SetSelectedMarker(GameObject marker)
    {
        selectedMarker = marker;
    }
    
    // 포지션변경 UI버튼 ON
    public void OnMoveModeButtonPressed()
    {
        isMoveMode = true;
        _markerUpdateDataUI.SetActive(false);
        _markerMoverUI.SetActive(true);
    }
    
    // 포지션변경 UI 버튼 OFF
    public void OffMoveModeButtonPressed()
    {
        isMoveMode = false;
        _markerUpdateDataUI.SetActive(true);
        HideArrowUI();
    }

    // 선택한 오브젝트 포지션변경 UI 보여주기
    public void ShowArrowUI(GameObject target)
    {
        selectedMarker = target;
    }

    // 포지션변경 UI OFF
    private void HideArrowUI()
    {
        _markerMoverUI.gameObject.SetActive(false);
    }

    // 포지션변경 공식
    public void MoveSelectedMarker(Vector3 direction)
    {
        if (selectedMarker == null)
        {
            return;
        }

        float moveAmount = 0.05f;
        selectedMarker.transform.position += direction * moveAmount;
    }

    //포지션 변경 XYZ버튼
    public void Xplus() => MoveSelectedMarker(Vector3.right);
    public void Xminus() => MoveSelectedMarker(Vector3.left);
    public void Yplus() => MoveSelectedMarker(Vector3.up);
    public void Yminus() => MoveSelectedMarker(Vector3.down);
    public void Zplus() => MoveSelectedMarker(Vector3.forward);
    public void Zminus() => MoveSelectedMarker(Vector3.back);
}

