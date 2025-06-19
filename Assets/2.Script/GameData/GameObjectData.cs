using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectData", menuName = "ScriptableObject/Data")]
public class GameObjectData : ScriptableObject
{
    [Serializable]
    public class GOData
    {
        public int id;
        public Vector3 position;
    }
    public List<GOData> GODatas = new List<GOData>();
}
