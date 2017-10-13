using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.Node
{
    public class NodeEditorState:ScriptableObject
    {
        public NodeGraph CurGraph;
        public Rect CurGraphRect;
        public Vector2 PanAdjust;
        public Vector2 ZoomPos;
        public Vector2 PanOffset=new Vector2();

        public bool IsPineSetting = false;
        public Vector2 DragStart=Vector2.zero;
        public Vector2 DragOffset=Vector2.zero;
        public Node SelectedNode;
        public Node FocusNode;
        public void UpdateData(Event e)
        {
            //TODO:Update State
        }
    }
}
