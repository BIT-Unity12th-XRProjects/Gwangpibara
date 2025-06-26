using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingUI : BaseUI
{

    private string loadingText = "Loading...";
    [SerializeField] private float textSpeed = 0.3f;
    public TextMeshProUGUI LoadingText;
    private void OnEnable()
    {
        StartCoroutine(CoDialog());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator CoDialog()
    {
        LoadingText.text = "L";
        int end = loadingText.Length;
        float timer = 0;
        
        int preIndex = -1;
        int curIndex = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= textSpeed)
            {
                timer = 0;
                curIndex = ((curIndex + 1) % end);

                if (preIndex != curIndex)
                {
                    if (curIndex == 0)
                    {
                        LoadingText.text = "";
                    }
                    LoadingText.text += loadingText[curIndex];
                    preIndex = curIndex;
                }
            }

            yield return null;
        }

        
    }
}
