using UnityEngine;
using UnityEngine.UI;

public class MarkerMover : MonoBehaviour
{
    public bool isMoveMode = false;
    [SerializeField] private GameObject _markerMoverUI;
    private GameObject selectedMarker;
    private Transform directionUITransform;
    [SerializeField] private GameObject _markerUpdateDataUI;
    
    public void OnMoveModeButtonPressed()
    {
        isMoveMode = true;
        _markerUpdateDataUI.SetActive(false);
        _markerMoverUI.SetActive(true);
    }

    public void OffMoveModeButtonPressed()
    {
        isMoveMode = false;
        selectedMarker = null;
        _markerUpdateDataUI.SetActive(true);
        HideArrowUI();
    }

    public void ShowArrowUI(GameObject target)
    {
        selectedMarker = target;
    }

    private void HideArrowUI()
    {
        _markerMoverUI.gameObject.SetActive(false);
    }

    public void MoveSelectedMarker(Vector3 direction)
    {
        if (selectedMarker == null) return;

        float moveAmount = 0.05f;
        selectedMarker.transform.position += direction * moveAmount;
    }

    public void Xplus() => MoveSelectedMarker(Vector3.right);
    public void Xminus() => MoveSelectedMarker(Vector3.left);
    public void Yplus() => MoveSelectedMarker(Vector3.up);
    public void Yminus() => MoveSelectedMarker(Vector3.down);
    public void Zplus() => MoveSelectedMarker(Vector3.forward);
    public void Zminus() => MoveSelectedMarker(Vector3.back);
}

