using UnityEngine;
using UnityEngine.InputSystem;

public class CheckItem : MonoBehaviour
{
    [SerializeField] private GameObject _textPrefab;
    [SerializeField] private string _name;
    private bool _canSpawn = true;

    private void Update()
    {
        if(_textPrefab != null)
        {
            if(Keyboard.current.gKey.wasPressedThisFrame && _canSpawn)
            {
                _canSpawn = false;
                CheckItemTest(null);
            }
        }
    }

    public void CheckItemTest(ItemData itemData)
    {
        _name = itemData.Name;
        Instantiate(_textPrefab, new Vector3(0,0,0), Quaternion.identity);
    }
}
