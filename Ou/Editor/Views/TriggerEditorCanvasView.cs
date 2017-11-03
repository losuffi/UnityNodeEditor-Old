
using Ou.Support.NodeSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    public class TriggerEditorCanvasView:ViewBase
    {
        private GenericMenu menu;


        public TriggerEditorCanvasView(string title) : base(title)
        {
        }
        public override void ProcessEvent(Event e)
        {
            base.ProcessEvent(e);
        }

        public override void UpdateView(Rect size, Rect percentageSize, Event e)
        {
            base.UpdateView(size, percentageSize, e);
            ProcessEvent(e);
            GUI.Box(ViewRect, Title, ViewSkin.GetStyle("TriggerEditorCanvas"));
            GUILayout.BeginArea(ViewRect);
            {
                if (e.button == 1 && e.type == EventType.mouseDown)
                {
                    menu = NodeEditor.GetGenericMenu();//需要修改 装入InputControls中
                    menu.ShowAsContext();
                }
                if(TriggerEditorUtility.CheckInit())
                    NodeEditor.DrawCanvas(ViewRect);
            }
            GUILayout.EndArea();
        }
    }
}
