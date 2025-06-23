using System;
using System.Collections.Generic;
using AREditor.LoadObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMarkerDataUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _markerName;
    [SerializeField] private TMP_InputField _changeDropId;
    [SerializeField] private TMP_InputField _changeAcquireStep;
    [SerializeField] private TMP_InputField _changeremoveStep;
    [SerializeField] private TMP_Dropdown _changeMarkerSpawnType;
    [SerializeField] private TMP_Dropdown _changeMarkerType;
    
    [SerializeField] private GameObject _panel;
    
    [SerializeField] private SaveMarker _saveMarker;
     
    private GameObject targetObject;
    
    private void Awake() 
    {
        _panel.SetActive(false);
        
        InitDropdown<MarkerSpawnType>(_changeMarkerSpawnType);
        InitDropdown<MarkerType>(_changeMarkerType);
    }

    public void Open(GameObject obj)
    {
        _panel.SetActive(true);
        targetObject = obj;
        
        MarkerData targetMarkerData = targetObject.GetComponentInParent<MarkerDataComponent>().markerData;
        
        _markerName.text = targetMarkerData.PrefabID.ToString();
        _changeDropId.text = targetMarkerData.DropItemID.ToString();
        _changeAcquireStep.text = targetMarkerData.AcquireStep.ToString();
        _changeremoveStep.text = targetMarkerData.RemoveStep.ToString();
        _changeMarkerSpawnType.value = (int)targetMarkerData.MarkerSpawnType;
        _changeMarkerType.value = (int)targetMarkerData.MarkerType;
    }

    public void ChangeMarkerData()
    {
        var targetMarkerData = targetObject.GetComponentInParent<MarkerDataComponent>();
        MarkerData data = targetMarkerData.markerData;

        data.PrefabID = Convert.ToInt32(_markerName.text);

        if (int.TryParse(_changeDropId.text, out var dropId))
        {
            data.DropItemID = dropId;
        }

        if (int.TryParse(_changeAcquireStep.text, out var acquireStep))
        {
            data.AcquireStep = acquireStep;
        }

        if (int.TryParse(_changeremoveStep.text, out var removeStep))
        {
            data.RemoveStep = removeStep;
        }

        data.MarkerSpawnType = (MarkerSpawnType)_changeMarkerSpawnType.value;
        data.MarkerType = (MarkerType)_changeMarkerType.value;
        
        data.Position = targetObject.transform.position;
        data.Rotation = targetObject.transform.rotation;
        data.Scale = targetObject.transform.localScale;
        
        _saveMarker.UpdateMarkerDataInList(data);
    }

    public void Close()
    {
        _panel.SetActive(false);
    }

    void InitDropdown<T>(TMP_Dropdown dropdown) where T : Enum
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(Enum.GetNames(typeof(T))));
    }
    
}


