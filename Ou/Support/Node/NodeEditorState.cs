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
    }
}
