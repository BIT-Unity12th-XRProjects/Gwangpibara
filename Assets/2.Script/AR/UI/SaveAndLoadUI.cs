using TMPro;
using UnityEngine;

namespace _2.Script.AR.UI
{
    public class SaveAndLoadUI : MonoBehaviour
    {
        [SerializeField] private SaveMarker _saveMarker;
        [SerializeField] private TMP_InputField _saveFileNameInputField;
        
        [SerializeField] private ARTrackingManager _arTrackingManager;
        [SerializeField] private TMP_InputField _loadFileNameInputField;

        [SerializeField] private GameObject _SaveMarkerUI;
        [SerializeField] private GameObject _loadMarkerUI;


        public void OnClickOpenSaveUI()
        {
            _SaveMarkerUI.SetActive(true);
            _loadMarkerUI.SetActive(false);
        }

        public void OnClickCloseSaveUI()
        {
            _SaveMarkerUI.SetActive(false);
        }

        public void OnClickOpenLoadUI()
        {
            _loadMarkerUI.SetActive(true);  
            _SaveMarkerUI.SetActive(false);
        }

        public void OnClickCloseLoadUI()
        {
            _loadMarkerUI.SetActive(false);
        }


        // 파일명 입력해서 저장        
        public void OnClickSaveButton()
        {
            string fileName = _saveFileNameInputField.text;

            if (string.IsNullOrEmpty(fileName) == false)
            {
                _saveMarker.SaveMarkerPosition(fileName);
            }
            _SaveMarkerUI.SetActive(false);
        }
        
        public void OnClickLoadButton()
        {
            string fileName = _loadFileNameInputField.text;
            
            if (string.IsNullOrEmpty(fileName) == false)
            {
                _arTrackingManager.LoadMarkers(fileName);
            }
            _loadMarkerUI.SetActive(false);
        }
    }

}