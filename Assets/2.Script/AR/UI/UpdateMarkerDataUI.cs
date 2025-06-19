using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMarkerDataUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _markerName;
    [SerializeField] private TMP_InputField _changeMarkerName;
    [SerializeField] private Button _applyButton;
    [SerializeField] private GameObject _panel;
     
    private GameObject targetObject;
    
    private void Awake()
    {
        _applyButton.onClick.AddListener(RenameMarker);
        _panel.SetActive(false);
    }

    public void Open(GameObject obj)
    {
        _panel.SetActive(true);
        targetObject = obj;
        Transform root = targetObject.transform.parent;
        _markerName.text = root.name;
    }
    
    public void Close()
    {
        _panel.SetActive(false);
    }

    // 이름변경 UI
    private void RenameMarker()
    {
        if (targetObject != null && !string.IsNullOrEmpty(_markerName.text))
        {
            string oldName = targetObject.transform.parent.name;
            string newName = _markerName.text;
            oldName = newName;

            UpdateEvents.UpdateMarkerStringData(oldName, newName);
        }
    }
    
    //DropID UI
    private void ReDropID()
    {
        if (targetObject != null && !string.IsNullOrEmpty(_changeMarkerName.text))
        {
            string oldName = targetObject.transform.parent.name;
            string newName = _changeMarkerName.text;
            oldName = newName;

            UpdateEvents.UpdateMarkerStringData(oldName, newName);
        }
    }
    //TODO : 모든 데이터 수정가능하도록 추가

}


