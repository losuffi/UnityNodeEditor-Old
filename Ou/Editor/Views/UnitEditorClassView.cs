using System.Collections.Generic;
using Ou.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    public class UnitEditorClassView:ViewBase
    {
        private UnitPool unitPool;
        private List<string> ClassNames;
        public int CurrentIndex;
        public UnitEditorClassView(string title) : base(title)
        {
        }
        public override void ProcessEvent(Event e)
        {
            base.ProcessEvent(e);
        }

        public void SetPool(UnitPool pool)
        {
            unitPool = pool;
            ClassNames=new List<string>();
            CurrentIndex = -1;
        }

        void UpdateDataName()
        {
            if (ClassNames.Count != unitPool.Pool.Count)
            {
                ClassNames.Clear();
                foreach (var c in unitPool.Pool)
                {
                    ClassNames.Add(c.Name);
                }
            }
        }
        public override void UpdateView(Rect size, Rect percentageSize, Event e)
        {
            base.UpdateView(size, percentageSize, e);
            GUI.Box(ViewRect, Title, ViewSkin.GetStyle("UnitEditorClassViewStyle"));
            GUILayout.BeginArea(ViewRect);
            {
                GUILayout.BeginVertical();
                GUILayout.Label("选择种类", ViewSkin.FindStyle("UnitEditorNormal"));
                UpdateDataName();
                CurrentIndex = EditorGUILayout.Popup(CurrentIndex, ClassNames.ToArray());
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("增减属性", ViewSkin.FindStyle("UnitEditorButton")))
                {
                    UnitEditorWindows.Instance.FieldsView.fieldViewType =
                        UnitEditorFieldsView.FieldViewType.UpdateFields;
                }
                if (GUILayout.Button("创建种类", ViewSkin.FindStyle("UnitEditorButton")))
                {
                    //创建种类
                    UnitEditorWindows.Instance.FieldsView.fieldViewType =
                        UnitEditorFieldsView.FieldViewType.AddClass;
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            ProcessEvent(e);
        }
    }
}
