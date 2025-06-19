using System;

[Serializable]
public class ThemeData
{
    public int themeNumber;
    public string title;
    public string description;

    // 문자열 배열로부터 초기화하는 생성자
    public ThemeData(string[] parseData)
    {
        int numberIdx = 0;
        int titleIdx = numberIdx + 1;
        int descIdx = titleIdx + 1;

        themeNumber = int.Parse(parseData[numberIdx]);
        title = parseData[titleIdx];
        description = parseData[descIdx];
    }

    // 복사 생성자
    public ThemeData(ThemeData origin)
    {
        themeNumber = origin.themeNumber;
        title = origin.title;
        description = origin.description;
    }
}

