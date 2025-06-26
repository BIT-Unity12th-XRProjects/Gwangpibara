
using TMPro;
using UnityEngine;

public class PopMessege : MonoBehaviour
{
    [SerializeField] private float _speed = 30f;
    [SerializeField] private float _lifeTime = 3f;
    public TextMeshProUGUI MessegeText;

    public void SetInfo(string messege, Vector3 position)
    {
        MessegeText.text = messege;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(position.x, position.y);
    }

    private void Update()
    {
        transform.localPosition += Vector3.up * _speed * Time.deltaTime;

        _lifeTime -= Time.deltaTime;
        if (_lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}

