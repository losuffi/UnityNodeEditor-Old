using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ou.Support.NodeSupport;

[System.Serializable]
public class UnitBase:ScriptableObject{
    public int ID;
    public string Name;
    public List<GlobalVariable> fields=new List<GlobalVariable>();
}
