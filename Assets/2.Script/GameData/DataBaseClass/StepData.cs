


using System;


public enum ClearType
{
    Click, Collect, Answer
}

public class StepData
{
    public int ID;
    public string PrintText;
    public string Hint;
    public ClearType ClearType;
    public string SuccessCode;

    public const int INVALID_NUMBER = -1;
    public int PreStep;
    public int NextStep;
    public int FailStep;

    public StepData(string[] parseData)
    {
        int pidIdx = 0;
        int printTextIdx = pidIdx + 1;
        int hintIdx = printTextIdx + 1;
        int ClearTypeIdx = hintIdx + 1;
        int successCodeIdx = ClearTypeIdx + 1;
        int preStepIdx = successCodeIdx + 1;
        int nextStepIdx = preStepIdx + 1;
        int failStepIdx = nextStepIdx + 1;

        ID = int.Parse(parseData[pidIdx]);
        PrintText = parseData[printTextIdx];
        Hint = parseData[hintIdx];
        ClearType = ParseEnum<ClearType>(parseData[ClearTypeIdx]);
        SuccessCode = parseData[successCodeIdx];
        PreStep = int.Parse(parseData[preStepIdx]);
        NextStep = int.Parse(parseData[nextStepIdx]);
        FailStep = (int.Parse(parseData[failStepIdx]));
    }

    private T ParseEnum<T>(string inEnumStr) where T : Enum
    {
        T parseEnum = (T)Enum.Parse(typeof(T), inEnumStr);
        return parseEnum;
    }

    // 복사 생성자
    public StepData(StepData origin)
    {
        ID = origin.ID;
        PrintText = origin.PrintText;
        Hint = origin.Hint;
        ClearType = origin.ClearType;
        SuccessCode = origin.SuccessCode;
        PreStep = origin.PreStep;
        NextStep = origin.NextStep;
        FailStep = origin.FailStep;
    }
}

