using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using Ou.Support.UnitSupport;
using UnityEngine;

namespace Ou.Editor.Views
{
    public class UnitEditorFieldView : ViewBase
    {
        public UnitEditorFieldView(string title) : base(title)
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
                UnitField.DrawField(ViewSkin);
            }
            GUILayout.EndArea();
        }
    }
}
