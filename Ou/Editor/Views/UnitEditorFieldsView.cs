using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Ou.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    public  class UnitEditorFieldsView:ViewBase
    {
        public enum FieldViewType
        {
            AddClass,
            UpdateFields,
            UpdateUnit,
            None,
        }

        public FieldViewType fieldViewType;
        public UnitEditorFieldsView(string title) : base(title)
        {
            fieldViewType = FieldViewType.None;
            PercentCompelete = 0;
            ClassName = string.Empty;
            scrollPositionField=Vector2.zero;
            fieldsName = string.Empty;
        }

        public override void ProcessEvent(Event e)
        {
            base.ProcessEvent(e);
        }

        public override void UpdateView(Rect size, Rect percentageSize, Event e)
        {
            base.UpdateView(size, percentageSize, e);
            GUI.Box(ViewRect, Title, ViewSkin.GetStyle("UnitEditorFieldsViewStyle"));
            GUILayout.BeginArea(ViewRect);
            {
                switch (fieldViewType)
                {
                    case FieldViewType.AddClass:
                        #region AddClass
                        GUILayout.BeginVertical();
                        GUILayout.Label("种类名（英文）：", ViewSkin.GetStyle("UnitEditorNormal"));
                        ClassName = GUILayout.TextField(ClassName, ViewSkin.textField);
                        GUILayout.Space(10);
                        if (GUI.Button(GUILayoutUtility.GetRect(180, 30), "生成"))
                        {
                            string Tempelte = File.ReadAllText(RootPath + @"Support/Unit/UnitPerson.cs");
                            Tempelte = Regex.Replace(Tempelte, @"UnitPerson", ClassName);
                            File.WriteAllText(RootPath + @"Wrapper/" + ClassName + ".cs", Tempelte);
                            UnitEditorWindows.Instance.ClassView.CurrentIndex  = -1;
                            var tar = new Pool(ClassName);
                            UnitEditorWindows.Instance.unitPool.Pool.Add(tar);
                            if (!UnitEditorWindows.Instance.IsInit)
                            {
                                UnitEditorWindows.Instance.IsInit = true;
                                AssetDatabase.CreateAsset(UnitEditorWindows.Instance.unitPool, @"Assets/Ou/Property/Unit.asset");
                            }
                            Start = true;
                        }
                        GUILayout.Space(20);
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
                        EditorGUI.ProgressBar(GUILayoutUtility.GetRect(180, 30), PercentCompelete, "进度");
                        GUILayout.EndVertical();
                        #endregion
                        break;
                    case FieldViewType.UpdateFields:
                        #region UpdateFields
                        var TargetName = UnitEditorWindows.Instance.unitPool.Pool[UnitEditorWindows.Instance.ClassView.CurrentIndex].Name;
                        UnitBase Target = Assembly.Load("Assembly-CSharp").CreateInstance(TargetName) as UnitBase;
                        var Fields = Target.GetType().GetFields();
                        using (var scr = new GUILayout.ScrollViewScope(scrollPositionField))
                        {
                            scrollPositionField = scr.scrollPosition;
                            GUILayout.BeginVertical();
                            for (int j = 0; j < Fields.Length; j++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(Fields[j].Name,ViewSkin.label);
                                GUILayout.Label(fieldsType[StringMetaToINT(Fields[j].FieldType.ToString())], ViewSkin.label);
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.Label("添加：",ViewSkin.GetStyle("UnitEditorNormal"));
                            fieldsName = GUILayout.TextField(fieldsName,ViewSkin.textField);
                            fieldType = EditorGUI.Popup(GUILayoutUtility.GetRect(100, 20), fieldType, fieldsType);
                            if (GUILayout.Button("+"))
                            {
                                metaInsert += "public" + " " + TypeIntTOStringMeta(fieldType) + " " + fieldsName +
                                              ";\n";
                            }
                            GUILayout.Box(metaInsert);
                            if (GUILayout.Button("注入"))
                            {
                                Start = true;
                                string Tempelte = File.ReadAllText(RootPath + @"Wrapper/" + TargetName + ".cs");
                                Tempelte = Regex.Replace(Tempelte, @"//OuTian", metaInsert + @"//OuTian");
                                Debug.Log(Tempelte);
                                File.WriteAllText(RootPath + @"Wrapper/" + TargetName + ".cs", Tempelte);
                            }
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
                            GUILayout.EndVertical();
                        }

                        #endregion
                            break;
                    case FieldViewType.UpdateUnit:
                        #region UpdateUnit
                        if (UnitEditorWindows.Instance.SelectedUnit == null)
                            return;
                        GUILayout.BeginVertical();
                        Fields = UnitEditorWindows.Instance.SelectedUnit.GetType().GetFields();
                        for (int j = 0; j < Fields.Length; j++)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(Fields[j].Name,ViewSkin.GetStyle("UnitEditorNormal"));
                            WriteFieldByString(Fields[j], UnitEditorWindows.Instance.SelectedUnit);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(40);
                        }
                        GUILayout.EndVertical();


                        #endregion
                        break;
                    case FieldViewType.None:
                        break;
                }
            }
            GUILayout.EndArea();
            ProcessEvent(e);
        }
        public static int StringMetaToINT(string type)
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
        public static string TypeIntTOStringMeta(int i)
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
        public void WriteFieldByString(FieldInfo f, UnitBase t)
        {
            switch (StringMetaToINT(f.FieldType.ToString()))
            {
                case 0:
                    f.SetValue(t,EditorGUILayout.IntField((int)f.GetValue(t), ViewSkin.textArea));
                    break;
                case 1:
                    f.SetValue(t, EditorGUILayout.TextField(f.GetValue(t) != null ? f.GetValue(t).ToString() : "",ViewSkin.textArea));
                    break;
                case 2:
                    f.SetValue(t, EditorGUILayout.FloatField((float)f.GetValue(t),ViewSkin.textArea));
                    break;
                default:
                    Debug.Log("Warnning!" + f.GetType().ToString());
                    break;
            }
        }
        public float PercentCompelete { get; set; }
        readonly string[] fieldsType = { "真值型", "字符串型", "实值型" };
        public bool Start { get; set; }
        private const string RootPath = @"Assets/Ou/";
        private Vector2 scrollPositionField;
        private string fieldsName;
        private int fieldType;
        private string metaInsert;
        public string ClassName { get; set; }
    }
}
