using System.Reflection;
using Ou.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    public class UnitEditorUnitView:ViewBase
    {
        private UnitBase CurrentUnit;
        private Vector2 scrollPosition;

        public UnitEditorUnitView(string title) : base(title)
        {
            CurrentUnit = null;
            scrollPosition=Vector2.zero;
        }

        public override void ProcessEvent(Event e)
        {
            base.ProcessEvent(e);
        }
        public override void UpdateView(Rect size, Rect percentageSize, Event e)
        {
            base.UpdateView(size, percentageSize, e);
            GUI.Box(ViewRect, Title, ViewSkin.GetStyle("UnitEditorUnitViewStyle"));
            GUILayout.BeginArea(ViewRect);
            {
                if (UnitEditorWindows.Instance.FieldsView.PercentCompelete != 0 ||
                    UnitEditorWindows.Instance.ClassView.CurrentIndex < 0)
                {
                    GUILayout.EndArea();
                    return;
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("添加单位", ViewSkin.GetStyle("UnitEditorButton")))
                {
                    if (UnitEditorWindows.Instance.unitPool == null)
                    {
                        return;
                    }
                    CurrentUnit = ScriptableObject.CreateInstance(UnitEditorWindows.Instance.unitPool.Pool[UnitEditorWindows.Instance.ClassView.CurrentIndex].Name) as UnitBase;
                    //CurrentUnit = Assembly.Load("Assembly-CSharp").CreateInstance(UnitEditorWindows.Instance.unitPool.Pool[UnitEditorWindows.Instance.ClassView.CurrentIndex].Name) as UnitBase;
                    UnitEditorWindows.Instance.unitPool.Pool[UnitEditorWindows.Instance.ClassView.CurrentIndex].datas.Add(CurrentUnit);
                    UnitEditorWindows.Instance.SelectedUnit = CurrentUnit;
                    AssetDatabase.AddObjectToAsset(CurrentUnit,
                        UnitEditorWindows.Instance.unitPool.Pool[UnitEditorWindows.Instance.ClassView.CurrentIndex]);
                    //AssetDatabase.CreateAsset(CurrentUnit,
                    //    @"Assets/Ou/Property/" +
                    //    UnitEditorWindows.Instance.unitPool.Pool[UnitEditorWindows.Instance.ClassView.CurrentIndex]
                    //        .Name + UnitEditorWindows.Instance.unitPool.Pool[UnitEditorWindows.Instance.ClassView.CurrentIndex].datas.Count + ".asset");
                    UnitEditorWindows.Instance.FieldsView.fieldViewType =
                        UnitEditorFieldsView.FieldViewType.UpdateUnit;
                }
                if (GUILayout.Button("更新/保存", ViewSkin.GetStyle("UnitEditorButton")))
                {
                    if (UnitEditorWindows.Instance.IsInit)
                    {
                        EditorUtility.SetDirty(UnitEditorWindows.Instance.unitPool);
                        AssetDatabase.SaveAssets();
                    }
                    else
                    {
                        UnitEditorWindows.Instance.IsInit = true;
                        AssetDatabase.CreateAsset(UnitEditorWindows.Instance.unitPool, @"Assets/Ou/Property/Unit.asset");
                    }
                }
                GUILayout.EndHorizontal();
                using (var scr = new GUILayout.ScrollViewScope(scrollPosition))
                {
                    scrollPosition = scr.scrollPosition;
                    if (UnitEditorWindows.Instance.unitPool.Pool.Count < 1)
                    {
                        return;
                    }
                    if (UnitEditorWindows.Instance.ClassView.CurrentIndex > UnitEditorWindows.Instance.unitPool.Pool.Count - 1)
                    {
                        UnitEditorWindows.Instance.ClassView.CurrentIndex = UnitEditorWindows.Instance.unitPool.Pool.Count - 1;
                    }
                    foreach (var c in UnitEditorWindows.Instance.unitPool.Pool[UnitEditorWindows.Instance.ClassView.CurrentIndex].datas)
                    {
                        if (c != null)
                        {
                            if (GUILayout.Button(c.Name))
                            {
                                UnitEditorWindows.Instance.SelectedUnit = c;
                                UnitEditorWindows.Instance.FieldsView.fieldViewType =
                                    UnitEditorFieldsView.FieldViewType.UpdateUnit;
                                AssetDatabase.Refresh();
                            }
                        }
                    }
                }
            }
            GUILayout.EndArea();
            ProcessEvent(e);
        }
    }
}
