using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    private string baseUrl = "http://localhost:5000";

    void Start()
    {
        StartCoroutine(GetMarkers());
    }

    IEnumerator GetMarkers()
    {
        string url = $"{baseUrl}/api/markers";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"API 호출 실패: {www.error}");
                yield break;
            }

            string json = www.downloadHandler.text;

        }
    }
}
