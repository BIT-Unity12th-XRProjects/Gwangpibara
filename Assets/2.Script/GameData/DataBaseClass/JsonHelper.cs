using System;
using UnityEngine;

public static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] marker;
    }

    public static T[] FromJson<T>(string json)
    {
        // JSON 배열 앞뒤에 키를 하나 붙여서 객체 형태로 만든 뒤 파싱
        string wrapped = $"{{ \"marker\": {json} }}";
        return JsonUtility.FromJson<Wrapper<T>>(wrapped).marker;
    }
}