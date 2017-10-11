using System;
using Ou.Editor.Views;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Windows
{
    public class TriggerEditorWindows:EditorWindow
    {
        public static TriggerEditorWindows Instance;

        public TriggerEditorAdjustView AdjustView;
        public TriggerEditorCanvasView CanvasView;
        public TriggerEditorToolBarView ToolBarView;

        private bool IsPaintDone;
        public static void Init()
        {
            Instance = GetWindow<TriggerEditorWindows>();
            Instance.titleContent = new GUIContent("TriggerEditor");
            Instance.maxSize = new Vector2(2000, 1600);
            Instance.minSize = new Vector2(1280, 800);
        }

        private void OnEnable()
        {
            IsPaintDone = false;
        }
        private void OnGUI()
        {
            if (!CheckView())
            {
                return;
            }
            Event e= Event.current;
            {
                if(e.type==EventType.Repaint&&!IsPaintDone)
                    return;
                AdjustView.UpdateView(new Rect(position.width, position.height, position.width, position.height),
                    new Rect(0, 0.1f, 0.2f, 0.901f),
                    e);
                CanvasView.UpdateView(new Rect(position.width, position.height, position.width, position.height),
                    new Rect(0.201f, 0.1f, 0.799f, 0.901f),
                    e);
                ToolBarView.UpdateView(position,
                    new Rect(0, 0, 1, 0.099f),
                    e);
                if (!IsPaintDone && e.type == EventType.Layout)
                {
                    IsPaintDone = true;
                }
            }
           Repaint();
        }

        bool CheckView()
        {
            if (Instance == null)
            {
                Init();
                return false;
            }
            if (AdjustView == null)
            {
                AdjustView=new TriggerEditorAdjustView("Adjust");
            }
            if (CanvasView == null)
            {
                CanvasView=new TriggerEditorCanvasView("Canvas");
            }
            if (ToolBarView == null)
            {
                ToolBarView=new TriggerEditorToolBarView("ToolBar");
            }
            return true;
        }
    }
}
