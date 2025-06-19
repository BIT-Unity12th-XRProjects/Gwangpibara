using System.Collections;
using TMPro;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;

    public void StartCountdown(System.Action onComplete)
    {
        StartCoroutine(C_Countdown(onComplete));
    }

    private IEnumerator C_Countdown(System.Action onComplete)
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "트래킹 완료!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

        onComplete?.Invoke();
    }
}
