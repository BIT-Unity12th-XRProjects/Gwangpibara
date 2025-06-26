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
    public List<string[]> DbValueList; //��񿡼� ���� ����

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
    //stat[] �� ����ϴ°�� db�� enum�� MatchValue�� ����� ���� � enum�� ������ 

    private Dictionary<EMasterData, ParseData> dbContainer = new(); //�Ľ��Ѱ��� �׳� ���� �ִ»��� - ����ϴ°����� �ٽ� ���� �ʿ�. 

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

        //��� �Ŵ������� Ŭ������ ������ֵ��� ������ �з�
        if (_successLoad)
        {
            //1. �Ľ��� Ÿ�� - �������� �������� ��Ī�صа�docuID - parseTypes
            EMasterData parseData = dbId[_index];
            //2. sheetData �ึ�� �и�
            string[] enterDivde = message.Split('\n'); //���� - �� �и�
            //3. ù��° ���� enum string�� �� db�� ������ ����� Į�������� ������� �κ�
            string[] dbEnumCode = enterDivde[0].Trim().Split('\t'); //enumCode�� Į�������� - ù��° ���� ����
  
            //5. ������ ���� ���� ����.
            List<string[]> dbValueList = new();
            for (int i = 1; i < enterDivde.Length; i++) //1����� �ڷ� ��
            {
                if (enterDivde[i][0].Equals('#'))
                {
                    //   Debug.Log(enterDivde[i] + "ù���� #���� �����ϴ� ���� �ǳʶ�");
                    continue;
                }


                string[] valueDivde = enterDivde[i].Trim().Split('\t'); //�� - �� �и� 

                // Debug.Log(parseData + "�� ������" + valueDivde.Length);
                dbValueList.Add(valueDivde);
            }
            //6. �Ľ��ڵ忡 - �ε��� ��Ī �ڵ�� ���� ������ struct�� ��� dctionary�� ���� 
            dbContainer.Add(parseData, new ParseData(dbValueList));

        }
        else
            Debug.LogWarning("�Ľ� ����");
    }

    public static IEnumerator GetSheetDataCo(string documentID, string[] sheetID, Action doneAct = null,
        Action<bool, int, string> process = null)
    {
        int doneWork = sheetID.Length; //ó���ؾ��� ����
        int curWork = 0;//ó���� ����
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

        doneAct?.Invoke(); //������ GameManager�� �۾� �Ϸ������� �˸�. 
    }
}
