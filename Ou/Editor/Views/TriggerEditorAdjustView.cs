
using Ou.Support.NodeSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    public  class TriggerEditorAdjustView:ViewBase
    {
        public TriggerEditorAdjustView(string title) : base(title)
        {

        }

        public override void ProcessEvent(Event e)
        {
            base.ProcessEvent(e);
            TriggerEditorUtility.ProcessE(e);
        }

        public override void UpdateView(Rect size, Rect percentageSize, Event e)
        {
            base.UpdateView(size, percentageSize, e);
            ProcessEvent(e);
            GUI.Box(ViewRect, Title, ViewSkin.GetStyle("TriggerEditorAdjust"));
            GUILayout.BeginArea(ViewRect);
            {
                if (TriggerEditorUtility.CheckInit())
                {
                    NodeAdjust.Draw(ViewSkin);
                }
            }
            GUILayout.EndArea();
        }
    }
}
