
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    public class TriggerEditorCanvasView:ViewBase
    {
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
                //TODO:ContexView
                if (e.button == 1 && e.type == EventType.mouseDown)
                {
                    GenericMenu menu=new GenericMenu();
                    menu.AddItem(new GUIContent("ADD Node"),false,CallBack,null);
                    menu.ShowAsContext();
                }
            }
            GUILayout.EndArea();
        }

        void CallBack(object obj)
        {
            
        }
    }
}
