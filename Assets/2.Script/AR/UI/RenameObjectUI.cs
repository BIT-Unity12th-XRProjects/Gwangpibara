using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RenameObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button applyButton;
    [SerializeField] private GameObject panel;
     
    private GameObject targetObject;
    
    private void Awake()
    {
        applyButton.onClick.AddListener(ApplyRename);
        panel.SetActive(false);
    }

    public void Open(GameObject obj)
    {
        panel.SetActive(true);
        targetObject = obj;
        Transform root = targetObject.transform.parent;
        inputField.text = root.name;
    }

    private void ApplyRename()
    {
        if (targetObject != null && !string.IsNullOrEmpty(inputField.text))
        {
            string oldName = targetObject.transform.parent.name;
            string newName = inputField.text;

            targetObject.transform.parent.name = newName;

            RenameEvents.RaiseRename(oldName, newName);
        }
    }

    public void Close()
    {
        //inputField.text = null;
        panel.SetActive(false);
    }
    
    
}

public static class RenameEvents
{
    public static event Action<string, string> OnMarkerRenamed;

    public static void RaiseRename(string oldName, string newName)
    {
        OnMarkerRenamed?.Invoke(oldName, newName);
    }
}
