
using Ou.Support.Node;
using UnityEngine;

namespace Ou.Editor.Views
{
    public class TriggerEditorToolBarView:ViewBase
    {
        public TriggerEditorToolBarView(string title) : base(title)
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
            GUI.Box(ViewRect, Title,ViewSkin.GetStyle("TriggerEditorToolBar"));
            GUILayout.BeginArea(ViewRect);
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear", GUILayout.Width(ViewRect.height), GUILayout.Height(ViewRect.height)))
                {
                    //TODO:Clear Node
                    NodeEditor.Clear();
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}
