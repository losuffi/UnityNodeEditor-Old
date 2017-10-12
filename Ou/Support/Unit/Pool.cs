using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Pool{
    public string Name;
    public List<UnitBase> datas;
    public Pool(string name)
    {
        Name = name;
    }
}
