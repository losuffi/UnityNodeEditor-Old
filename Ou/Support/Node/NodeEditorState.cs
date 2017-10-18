using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.Node
{
    public enum EditorType
    {
        Tree,
        StateMachine,
    }
    public class NodeEditorState:ScriptableObject
    {
        public NodeGraph CurGraph;
        public Rect CurGraphRect;
        public Vector2 PanAdjust=Vector2.zero;
        public Vector2 ZoomPos;
        public Vector2 PanOffset=new Vector2();

        public bool IsPineSetting = false;
        public Vector2 DragStart=Vector2.zero;
        public Vector2 DragOffset=Vector2.zero;
        public Node SelectedNode;
        public Node FocusNode;
        public NodeKnob FocusKnob;
        public NodeKnob SelectedKnob;
        public bool IsLinkSetting = false;
        public void UpdateData(Event e)
        {
            //TODO:Update State
        }

        public bool IsHaveDataAsset = false;
        public float GraphZoom = 1;

        #region AdjustData

        [NonSerialized]
        public bool IsTree = true;
        [NonSerialized]
        public bool IsStateMachine = false;

        public EditorType? editorType = EditorType.Tree;

        public void SelectedEditorType(EditorType type,ref bool _switch)
        {
            editorType = type;
            IsTree = false;
            IsStateMachine = false;
            _switch = true;
        }
        #endregion
    }
}
