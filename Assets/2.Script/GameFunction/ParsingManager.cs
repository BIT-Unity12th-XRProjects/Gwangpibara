using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public enum EMasterData
{
    ThemeData, StepData, ItemData
}
public struct ParseData
{
    public List<string[]> DbValueList; //디비에서 따온 값들

    public ParseData(List<string[]> _dbValues)
    {
        DbValueList = _dbValues;
    }

    public void NullCheck()
    {
        if (DbValueList == null)
        {
            DbValueList = new();
        }
    }
}

public class ParsingManager : Singleton<ParsingManager>
{
    private static string docuIDes = "1YhAAtfAxhKYbmnN7miPnWYcRdcilNYVHe2M-ixUejGU";
    private string[] sheetIDes = { "1191431163", "0", "1559678473"
                                   };
    private EMasterData[] dbId = { EMasterData.ThemeData, EMasterData.StepData, EMasterData.ItemData};
    //stat[] 를 사용하는경우 db에 enum값 MatchValue를 만들기 위해 어떤 enum을 쓰는지 

    private Dictionary<EMasterData, ParseData> dbContainer = new(); //파싱한값을 그냥 갖고만 있는상태 - 사용하는곳에서 다시 가공 필요. 

    public override void Awake()
    {
        base.Awake();
    }

    public ParseData GetMasterData(EMasterData _dataId)
    {
        dbContainer.TryGetValue(_dataId, out ParseData _parseData);
        _parseData.NullCheck();
        return _parseData;
    }

    public IEnumerator ParseSheetData()
    {
        yield return StartCoroutine(
           GetSheetDataCo(docuIDes, sheetIDes,
         delegate {
            
         },
         ClassfyDataBase)
            );
    }

    private void ClassfyDataBase(bool _successLoad, int _index, string message)
    {

        //담당 매니저에서 클래스를 만들수있도록 데이터 분류
        if (_successLoad)
        {
            //1. 파싱한 타입 - 수동으로 변수에서 매칭해둔거docuID - parseTypes
            EMasterData parseData = dbId[_index];
            //2. sheetData 행마다 분리
            string[] enterDivde = message.Split('\n'); //엔터 - 행 분리
            //3. 첫번째 행은 enum string값 중 db로 관리할 목록을 칼럼명으로 적어놓은 부분
            string[] dbEnumCode = enterDivde[0].Trim().Split('\t'); //enumCode를 칼럼명으로 - 첫번째 행의 역할
  
            //5. 나머지 행은 실제 값들.
            List<string[]> dbValueList = new();
            for (int i = 1; i < enterDivde.Length; i++) //1행부터 자료 값
            {
                if (enterDivde[i][0].Equals('#'))
                {
                    //   Debug.Log(enterDivde[i] + "첫열이 #으로 시작하는 행은 건너띔");
                    continue;
                }


                string[] valueDivde = enterDivde[i].Trim().Split('\t'); //탭 - 열 분리 

                // Debug.Log(parseData + "행 사이즈" + valueDivde.Length);
                dbValueList.Add(valueDivde);
            }
            //6. 파싱코드에 - 인덱스 매칭 코드와 실제 값들을 struct로 묶어서 dctionary에 저장 
            dbContainer.Add(parseData, new ParseData(dbValueList));

        }
        else
            Debug.LogWarning("파싱 실패");
    }

    public static IEnumerator GetSheetDataCo(string documentID, string[] sheetID, Action doneAct = null,
        Action<bool, int, string> process = null)
    {
        int doneWork = sheetID.Length; //처리해야할 숫자
        int curWork = 0;//처리한 숫자
        while (curWork < doneWork)
        {
            string url = $"https://docs.google.com/spreadsheets/d/{documentID}/export?format=tsv&gid={sheetID[curWork]}";

            UnityWebRequest req = UnityWebRequest.Get(url);

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ConnectionError || req.responseCode != 200)
            {

                process?.Invoke(false, curWork, null);
            }
            else
            {
                process?.Invoke(true, curWork, req.downloadHandler.text);
            }

            curWork += 1;
        }

        doneAct?.Invoke(); //보통은 GameManager에 작업 완료했음을 알림. 
    }
}
