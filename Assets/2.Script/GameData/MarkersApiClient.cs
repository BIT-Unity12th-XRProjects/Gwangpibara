using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;

public class MarkersApiClient : MonoBehaviour
{

    private string _baseUrl = "http://192.168.0.28:5000";

/*    private void Start() //테스트용
    {
        StartCoroutine(GetAllMarkers(
            (arr) => Debug.Log(arr[0]),
            (err) => { Debug.Log(err); }));
    }
*/    public IEnumerator GetMarkerById(int id, Action<ServerMarkerData> onSuccess, Action<string> onError)
    {
        var url = $"{_baseUrl}/api/markers/{id}";
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                switch (req.responseCode)
                {
                    case 200:
                        try
                        {
                            var marker = JsonConvert.DeserializeObject<ServerMarkerData>(req.downloadHandler.text);
                            onSuccess?.Invoke(marker);
                        }
                        catch (Exception ex)
                        {
                            onError?.Invoke($"JSON 파싱 실패: {ex.Message}");
                        }
                        break;

                    case 404:
                        onError?.Invoke("마커를 찾을 수 없습니다 (404 Not Found).");
                        break;

                    default:
                        onError?.Invoke($"예상치 못한 응답 코드: {req.responseCode}");
                        break;
                }
            }
            else
            {
                onError?.Invoke($"[{req.responseCode}] {req.error}");
            }
        }
    }

    /// <summary>
    /// 저장된 모든 마커 조회
    /// GET /api/markers
    /// </summary>
    public IEnumerator GetAllMarkers(Action<ServerMarkerData[]> onSuccess, Action<string> onError)
    {
        string url = $"{_baseUrl}/api/markers";
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;

                ServerMarkerData[] markers = JsonConvert.DeserializeObject<ServerMarkerData[]>(json);
                Debug.Log(json);
                onSuccess?.Invoke(markers);
            }
            else
            {
                onError?.Invoke(req.error);
            }
        }
    }

    public IEnumerator CreateMarker(ServerMarkerData newMarker, Action<ServerMarkerData[]> onSuccess, Action<string> onError)
    {
        string json = JsonConvert.SerializeObject(newMarker);
        string url = $"{_baseUrl}/api/markers";

        using (UnityWebRequest www = UnityWebRequest.Post(url, json, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var created = JsonConvert.DeserializeObject<ServerMarkerData[]>(www.downloadHandler.text);
                onSuccess?.Invoke(created);
                Debug.LogError(www.error);
            }
            else
            {
                onError?.Invoke($"JSON 파싱 실패:");
            }
        }
        /*using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
 
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var created = JsonConvert.DeserializeObject<ServerMarkerData>(req.downloadHandler.text);
                    onSuccess?.Invoke(created);
                }
                catch (Exception ex)
                {
                    onError?.Invoke($"JSON 파싱 실패: {ex.Message}");
                }
            }
            else
            {
                string err = $"[{req.responseCode}] {req.error}";
                onError?.Invoke(err);
            }
        }*/
    }

    /// <summary>
    /// MarkerData 배열 전체를 한 번에 서버로 전송해 일괄 업데이트
    /// </summary>
    public IEnumerator UpdateMarkersBulk(ServerMarkerData[] markers)
    {
        string json = JsonConvert.SerializeObject(markers);

        string url = $"{_baseUrl}/api/markers/bulk";
        using UnityWebRequest req = new UnityWebRequest(url, "PUT");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError($"일괄 업데이트 실패: {req.error}");
        else
            Debug.Log("일괄 업데이트 성공!");
    }

    /// <summary>
    /// 해당 id의 마커 삭제
    /// </summary>
    /// <param name="id">마커 id</param>
    /// <returns></returns>
    public IEnumerator DeleteMark(int id, Action onSuccess, Action<string> onError)
    {
        string url = $"{_baseUrl}/api/markers/{id}";
        using (UnityWebRequest req = UnityWebRequest.Delete(url))
        {
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success || req.responseCode == 204)
            {
                onSuccess?.Invoke();
            }
            else
            {
                string err = $"[{req.responseCode}] {req.error}";
                onError?.Invoke(err);
            }
        }

    }
}

