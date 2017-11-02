using System;
using System.Collections.Generic;
using System.Reflection;
using Ou.Editor.Views;
using Ou.Support.UnitSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Windows
{
    public class UnitEditorWindows:EditorWindow
    {
        public UnitEditorToolBarView ToolBarView;
        public UnitEditorFieldView FieldsView;
        public static UnitEditorWindows Instance;

        public bool IsPaintDone;
        public static void Init()
        {
            Instance = GetWindow<UnitEditorWindows>(true);
            Instance.maxSize = new Vector2(300, 1000);
            Instance.minSize = new Vector2(300, 1000);
            Instance.titleContent = new GUIContent("UnitEditor");
        }

        void OnEnable()
        {
            IsPaintDone = false;
            UnitEditor.StartInterrupted();
        }

        void OnGUI()
        {
            if (!CheckView())
            {
                return;
            }
            Event e = Event.current;
            {
                if (e.type == EventType.Repaint && !IsPaintDone)
                    return;
                //Draw SubWindow
                if (!Application.isPlaying)
                {
                    try
                    {
                        DrawViews(e);
                    }
                    catch (ArgumentException exception)
                    {
                        Debug.Log(exception);
                    }
                }
                if (!IsPaintDone && e.type == EventType.Layout)
                {
                    IsPaintDone = true;
                }
            }
            Repaint();
        }

        void DrawViews(Event e)
        {
            ToolBarView.UpdateView(new Rect(position.width, position.height, position.width, position.height),
                new Rect(0, 0, 1, 0.039f),
                e);
            FieldsView.UpdateView(new Rect(position.width, position.height, position.width, position.height),
                new Rect(0, 0.04f, 1, 0.96f),
                e);
        }
        private bool CheckView()
        {
            if (Instance == null)
            {
                Init();
                return false;
            }
            if (ToolBarView == null)
            {
                Instance.ToolBarView = new UnitEditorToolBarView("ToolBar View");
            }
            if (FieldsView == null)
            {
                Instance.FieldsView=new UnitEditorFieldView("Fields View");
            }
            return true;
        }
    }
}
