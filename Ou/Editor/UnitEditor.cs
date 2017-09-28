using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
public class UnitEditor : EditorWindow {
    [MenuItem("Ou/UnitEditor")]
    static void Init()
    {
        EditorWindow.GetWindow<UnitEditor>("UnitEditor");
    }
    public Vector2 scrollPosition = Vector2.zero;
    Vector2 scrollPositionName =Vector2.zero;
    Vector2 scrollPositionField = Vector2.zero;
    Texture Tex,WindowTex;
    Rect UnitWindow, ADDUNIT, ADDCLASS, Class, ADDFIELDS, READUNIT;
    GUIStyle style;
    UnitPool unitpool;
    UnitBase CurrentUnit;
    int CurrentKindIndex;
    enum WorkKind
    {
        AddUnit,
        AddField,
        AddClass,
        ReadUnit,
        None,
    }
    WorkKind workkind;
    const string RootPath = @"Assets/Ou/";
    bool IsInit ;
    private void OnEnable()
    {
        Tex = AssetDatabase.LoadAssetAtPath<Texture>(RootPath+@"OuSource/UnitBg.png");
        WindowTex= AssetDatabase.LoadAssetAtPath<Texture>(RootPath+@"OuSource/UnitWindow.png");
        unitpool = AssetDatabase.LoadAssetAtPath<UnitPool>(RootPath+ @"Property/Unit.asset");
        CurrentUnit = null;
        SelectedUnit = null;
        IsInit = (unitpool != null);
        if (unitpool == null)
        {
            unitpool = ScriptableObject.CreateInstance<UnitPool>();
        }
        workkind = WorkKind.None;
        style = new GUIStyle();
        CurrentKindIndex = -1;
        if (Temp.Count > 0)
        {
            var datas = unitpool.Pool[TempIndex].datas;
            for (int j = 0; j < Temp.Count; j++)
            {
                if (Temp[j].GetType().ToString().Equals("UnitBase"))
                {
                    var T = Assembly.Load("Assembly-CSharp").CreateInstance(unitpool.Pool[TempIndex].Name) as UnitBase;
                    T.Clone(Temp[j]);
                    datas.Add(T);
                }
            }
            TempIndex = -1;
            Temp.Clear();
        }
    }
    private void OnGUI()
    {
        BeginWindows();
        Class = GUI.Window(6, new Rect(0, 0, 200, this.position.height / 3), ClassPanel, "Class");
        UnitWindow =GUI.Window(0, new Rect(0, this.position.height / 3, 200, this.position.height * 2 / 3), UnitPanel, "Unit");
        // GUI.DrawTexture(new Rect(0,0, this.position.width,this.position.height), WindowTex);
        if (workkind== WorkKind.AddUnit)
        {
            ADDUNIT = GUI.Window(1, new Rect(200, 0, this.position.width-200, this.position.height), ViewUnit, "Unit  Edit");
        }
        else if (workkind == WorkKind.AddField)
        {
            ADDFIELDS = GUI.Window(4, new Rect(200, 0, this.position.width - 200, this.position.height), AddFields, "Add  Field");
        }
        else if(workkind== WorkKind.AddClass)
        {
            ADDCLASS = GUI.Window(3, new Rect(200, 0, this.position.width - 200, this.position.height), AddClass, "Add  Class");
        }else if(workkind == WorkKind.ReadUnit)
        {
            READUNIT= GUI.Window(5, new Rect(200, 0, this.position.width - 200, this.position.height), ReadUnit, "Read  Unit");
        }
        EndWindows();
    }
    List<string> strs = new List<string>();
    void ClassPanel(int i)
    {
        if (PercentCompelete != 0)
            return;
        style.fontSize = 18;
        style.normal.textColor = Color.black;
        GUILayout.Label("选择种类", style);
        strs.Clear();
        foreach(var c in unitpool.Pool)
        {
            strs.Add(c.Name);
        }
        CurrentKindIndex=EditorGUI.Popup(new Rect(0, 40, 200, 15), CurrentKindIndex, strs.ToArray());
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("增减属性"))
        {
            fieldType = 0;
            metaInsert = "";
            fieldsName = "";
            workkind = WorkKind.AddField;
            if (unitpool == null)
            {
                return;
            }
        }
        if (GUILayout.Button("创建种类"))
        {
            workkind = WorkKind.AddClass;
            if (unitpool == null)
            {
                return;
            }
        }
        GUILayout.EndHorizontal(); 
    }
    void UnitPanel(int i)
    {
        if (PercentCompelete != 0||CurrentKindIndex<0)
            return;
        Rect thisWindow = new Rect(0, 0, 200, this.position.height * 2 / 3);
        style.normal.textColor = Color.white;
        var ButtonStyle = GUI.skin.button;
        ButtonStyle.normal.textColor = Color.black;
        // GUI.DrawTexture(thisWindow, Tex);
        style.fontSize = 30;
        if (GUILayout.Button("添加单位"))
        {
            if (unitpool == null)
            {
                return;
            }
            CurrentUnit = Assembly.Load("Assembly-CSharp").CreateInstance(unitpool.Pool[CurrentKindIndex].Name) as UnitBase;
            unitpool.Pool[CurrentKindIndex].datas.Add(CurrentUnit);
            workkind = WorkKind.AddUnit;
        }
        if (GUILayout.Button("更新/保存"))
        {
            workkind = WorkKind.None;
            //if (unitpool != null&&CurrentUnit!=null)
            //{
            //    unitpool.Pool.Add(CurrentUnit);
            if (IsInit)
            {
                EditorUtility.SetDirty(unitpool);
                AssetDatabase.SaveAssets();
            }
            else
            {
                IsInit = true;
                AssetDatabase.CreateAsset(unitpool, @"Assets/Ou/Property/Unit.asset");
            }
        }
        using (var scr = new GUILayout.ScrollViewScope(scrollPosition))
        {
            scrollPosition = scr.scrollPosition;
            if (unitpool.Pool.Count < 1)
            {
                return;
            }
            if (CurrentKindIndex > unitpool.Pool.Count - 1)
            {
                CurrentKindIndex = unitpool.Pool.Count - 1;
            }
            foreach (var c in unitpool.Pool[CurrentKindIndex].datas)
            {
                if (c != null)
                {
                    if (GUILayout.Button(c.Name))
                    {
                        SelectedUnit = c;
                        workkind = WorkKind.ReadUnit;
                    }
                }
            }
        }
    }
    void ViewUnit(int i)
    {
        GUILayout.BeginVertical();
        var Fields = CurrentUnit.GetType().GetFields();
        for(int j = 0; j < Fields.Length; j++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Fields[j].Name);
            WriteFieldByString(Fields[j], CurrentUnit);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
    UnitBase SelectedUnit;
    void ReadUnit(int i)
    {
        if (SelectedUnit == null)
            return;
        GUILayout.BeginVertical();
        var Fields = SelectedUnit.GetType().GetFields();
        for (int j = 0; j < Fields.Length; j++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Fields[j].Name);
            WriteFieldByString(Fields[j], SelectedUnit);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
    string ClassName = "";
    bool Start = false;
    float PercentCompelete = 0;
    void AddClass(int i)
    {
        style.normal.textColor = Color.black;
        style.fontSize = 30;
        GUILayout.BeginVertical();
        GUILayout.Space(40);
        GUILayout.Label("种类名（英文）：", style);
        ClassName = GUILayout.TextField(ClassName);
        GUILayout.Space(10);
        if (GUI.Button(GUILayoutUtility.GetRect(180,30),"生成"))
        {
            string Tempelte = File.ReadAllText(RootPath + @"Support/UnitPerson.cs");
            Tempelte = Regex.Replace(Tempelte, @"UnitPerson", ClassName);
            File.WriteAllText(RootPath+@"Wrapper/" + ClassName + ".cs", Tempelte);
            CurrentKindIndex = -1;
            unitpool.Pool.Add(new Pool(ClassName));
            if (!IsInit)
            {
                IsInit = true;
                AssetDatabase.CreateAsset(unitpool, @"Assets/Ou/Property/Unit.asset");
            }
            Start = true;
        }
        GUILayout.Space(50);
        if (Start)
        {
            PercentCompelete += 0.01f;
            if (PercentCompelete > 1)
            {
                PercentCompelete = 0;
                AssetDatabase.Refresh();
                Start = false;
            }
        }
        EditorGUI.ProgressBar(GUILayoutUtility.GetRect(180, 20), PercentCompelete, "进度");
        Repaint();
        GUILayout.EndVertical();
    }
    string fieldsName = "";
    int fieldType;
    string[] fieldsType = { "真值型", "字符串型","实值型" };
    string metaInsert = "";
    List<UnitBase> Temp = new List<UnitBase>();
    int TempIndex = 0;
    void AddFields(int i)
    {
        var TargetName = unitpool.Pool[CurrentKindIndex].Name;
        UnitBase Target = Assembly.Load("Assembly-CSharp").CreateInstance(TargetName) as UnitBase;
        var Fields = Target.GetType().GetFields();
        using (var scr = new GUILayout.ScrollViewScope(scrollPositionField))
        {
            scrollPositionField = scr.scrollPosition;
            GUILayout.BeginVertical();
            for (int j = 0; j < Fields.Length; j++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(Fields[j].Name);
                GUILayout.Label(fieldsType[StringMetaToINT(Fields[j].FieldType.ToString())] );
                GUILayout.EndHorizontal();
            }
            GUILayout.Label("添加："); 
            fieldsName = GUILayout.TextField(fieldsName);
            fieldType = EditorGUI.Popup(GUILayoutUtility.GetRect(100, 20), fieldType, fieldsType);
            if (GUILayout.Button("+"))
            {
                metaInsert += "public"+" "+ TypeIntTOStringMeta(fieldType) + " " + fieldsName + ";\n";
            }
            GUILayout.Box(metaInsert);
            if (GUILayout.Button("注入"))
            {
                Start = true;
                string Tempelte = File.ReadAllText(RootPath + @"Wrapper/" + TargetName + ".cs");
                Tempelte = Regex.Replace(Tempelte, @"//OuTian", metaInsert + @"//OuTian");
                File.WriteAllText(RootPath + @"Wrapper/" + ClassName + ".cs", Tempelte);
            }
            if (Start)
            {
                PercentCompelete += 0.01f;
                if (PercentCompelete > 1)
                {
                    PercentCompelete = 0;
                    AssetDatabase.Refresh();
                    var datas = unitpool.Pool.Find(z => z.Name.Equals(TargetName)).datas;
                    for(int j = 0; j < datas.Count; j++)
                    {
                        var temp = Assembly.Load("Assembly-CSharp").CreateInstance(TargetName) as UnitBase;
                        temp.Clone(datas[j]);
                        Temp.Add(temp);
                    }
                    TempIndex = CurrentKindIndex;
                    datas.Clear();
                    Start = false;
                }
            }
            EditorGUI.ProgressBar(GUILayoutUtility.GetRect(180, 20), PercentCompelete, "进度");
            Repaint();
            GUILayout.EndVertical();
        }
    }
    string TypeIntTOStringMeta(int i)
    {
        switch (i)
        {
            case 0:
                return "System.Int32";
            case 1:
                return "System.String";
            case 2:
                return "System.Single";
            default:
                return "";
        }
    }
    int StringMetaToINT(string type)
    {
        switch (type)
        {
            case "System.Int32":
                return 0;
            case "System.String":
                return 1;
            case "System.Single":
                return 2;
            default:
                return -1;
        }
    }
    void WriteFieldByString(FieldInfo f, UnitBase t)
    {
        switch (StringMetaToINT(f.FieldType.ToString()))
        {
            case 0:
                f.SetValue(t, EditorGUILayout.IntField((int)f.GetValue(t)));
                break;
            case 1:
                f.SetValue(t, EditorGUILayout.TextField(f.GetValue(t) != null ? f.GetValue(t).ToString() : ""));
                break;
            case 2:
                f.SetValue(t, EditorGUILayout.FloatField((float)f.GetValue(t)));
                break;
            default:
                Debug.Log("Warnning!"+f.GetType().ToString());
                break;
        }
    }
}
