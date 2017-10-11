using System;
using System.Collections.Generic;
using System.Reflection;
using Ou.Editor.Views;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Windows
{
    public class UnitEditorWindows:EditorWindow
    {
        public UnitEditorClassView ClassView;
        public UnitEditorUnitView UnitView;
        public UnitEditorFieldsView FieldsView;
        public static UnitEditorWindows Instance;

        public  UnitPool unitPool;
        public bool IsInit;
        private bool LayoutisDone;
        public UnitBase SelectedUnit;
        public static void Init()
        {
            Instance = GetWindow<UnitEditorWindows>();
            Instance.titleContent = new GUIContent("UnitEditor");
        }

        void OnEnable()
        {
            LayoutisDone = false;
            SelectedUnit = null;
            unitPool = AssetDatabase.LoadAssetAtPath<UnitPool>(@"Assets/Ou/Property/Unit.asset");
            IsInit = unitPool != null;
            if (!IsInit)
            {
                unitPool = ScriptableObject.CreateInstance<UnitPool>();
            }
        }

        void OnGUI()
        {
            if (!CheckView())
            {
                return;
            }
            Event e=Event.current;
            ClassView.UpdateView(new Rect(position.width, position.height, position.width, position.height),
                new Rect(0, 0, 0.2f, 0.35f),
                e);
            UnitView.UpdateView(new Rect(position.width, position.height, position.width, position.height),
                new Rect(0, 0.36f, 0.2f, 0.65f),
                e);
            FieldsView.UpdateView(new Rect(position.width, position.height, position.width, position.height),
                new Rect(0.21f, 0.01f, 0.78f, 0.98f),
                e);
            Repaint();
        }

        private void ViewsConnecttion()
        {

        }

        private bool CheckView()
        {
            if (Instance == null)
            {
                Init();
                return false;
            }
            if (ClassView == null)
            {
                Instance.ClassView = new UnitEditorClassView("Class View");
                Instance.ClassView.SetPool(unitPool);
            }
            if (UnitView == null)
            {
                Instance.UnitView = new UnitEditorUnitView("Unit View");
            }
            if (FieldsView == null)
            {
                Instance.FieldsView=new UnitEditorFieldsView("Fields View");
            }
            ViewsConnecttion();
            return true;
        }
    }
}
