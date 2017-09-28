using UnityEngine;
using System.Collections;
using UnityEditor;
public class Wrapper{
    [MenuItem("Tools/Wrapper")]
    static void Init()
    {
        Test1 sd = ScriptableObject.CreateInstance<Test1>();
        sd.content = new System.Collections.Generic.List<Vector2>();
        sd.content.Add(new Vector2(10, 20));
        sd.content.Add(new Vector2(30, 40));
        string p = "Assets/SysData.asset";
        AssetDatabase.CreateAsset(sd, p);
        Object o = AssetDatabase.LoadAssetAtPath(p, typeof(Test1));
        //BuildPipeline.BuildAssetBundles("SysData.assetbundle");
        //BuildPipeline.BuildAssetBundle(o, null, "SysData.assetbundle");
    }
}
