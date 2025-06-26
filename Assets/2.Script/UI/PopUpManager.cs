using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : Singleton<PopUpManager>
{

    [SerializeField] private PopMessege _messegePrefab;

    public void PopMessege(string messegeStr)
    {
        PopMessege messege = Instantiate(_messegePrefab, transform);

        // RectTransform 정보 가져오기
        RectTransform canvasRect = GetComponent<Canvas>().GetComponent<RectTransform>();
        RectTransform messageRect = messege.GetComponent<RectTransform>();

        // 텍스트 먼저 설정
        messege.MessegeText.text = messegeStr;
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);

        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        float msgWidth = messageRect.rect.width;
        float msgHeight = messageRect.rect.height;

        // 중앙 영역 안에서 랜덤 위치
        float minX = -((canvasWidth - msgWidth) / 2f);
        float maxX = ((canvasWidth - msgWidth) / 2f);
        float minY = -((canvasHeight - msgHeight) / 2f);
        float maxY = ((canvasHeight - msgHeight) / 2f);

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 randomPos = new Vector3(randomX, randomY, 0f);

        // 메시지 위치 설정
        messege.SetInfo(messegeStr, randomPos);
    }
}
