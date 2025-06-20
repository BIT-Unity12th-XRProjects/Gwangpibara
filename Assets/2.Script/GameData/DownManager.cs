using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class DownManager : MonoBehaviour
{
    public Slider DownSlider;
    public TextMeshProUGUI SizeInfo;
    public TextMeshProUGUI DownValText;

    public AssetLabelReference ARLabel;

    private long patchSize;
    private Dictionary<string, long> patchMap = new Dictionary<string, long>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CheckUpdateFiles());
    }

    IEnumerator CheckUpdateFiles()
    {
        List<string> labels = new List<string>() { ARLabel.labelString };

        patchSize = default;

        foreach (string label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle;

            patchSize += handle.Result;
        }

        if (patchSize > decimal.Zero)
        {
            //Down ...

            SizeInfo.text = GetFileSize(patchSize);
        }
        else
        {
            DownValText.text = "100 %";
            DownSlider.value = 1f;
        }
    }

    private string GetFileSize(long byteCnt)
    {
        string size = "0 Bytes";
        if (byteCnt >= 1073741824.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1073741824.0) + " GB";
        }
        else if (byteCnt >= 1048576.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1048576.0) + " MB";
        }
        else if (byteCnt >= 1024.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1024.0) + " KB";
        }
        else if (byteCnt > 0)
        {
            size = byteCnt.ToString() + " Bytes";
        }

        return size;
    }

    public void OnDownloadClicked()
    {
        StartCoroutine(PatchFiles());
    }
    IEnumerator PatchFiles()
    {
        List<string> labels = new List<string>() { ARLabel.labelString };

        foreach (string label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle;

            if (handle.Result != decimal.Zero)
            {
                StartCoroutine(DownLoadLabel(label));
            }
        }

        yield return CheckDownLoad();
    }

    IEnumerator CheckDownLoad()
    {
        var total = 0f;
        DownValText.text = "0 %";

        while (true)
        {
            total += patchMap.Sum(tmp => tmp.Value); //모든 다운로드 진행 상황을 총합으로 나타냄

            DownSlider.value = total / patchSize;
            DownValText.text = (int)(DownSlider.value * 100) + " %";

            if (total == patchSize)
            {
                //씬 전환 등...
                break;
            }
            total = 0f;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator DownLoadLabel(string label)
    {
        patchMap.Add(label, 0); //각 레이블에 대한 다운로드 상태 저장용

        AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame(); //다운로드 완료될 때까지 너무 많은 연산 자원 소모하지 않도록
        }

        patchMap[label] = handle.GetDownloadStatus().TotalBytes; //다운로드 상태의 TotalBytes를 대입
        Addressables.Release(handle);
    }
}
