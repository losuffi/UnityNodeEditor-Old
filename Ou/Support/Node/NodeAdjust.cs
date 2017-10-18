using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.OuUtility;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Node
{
    public static class NodeAdjust
    {
        public static void Draw(GUISkin skin)
        {
            #region Handle
            GUILayout.Label("---操作---", skin.GetStyle("adjustBodyLabel"));
            OuUIUtility.FormatButton("Clear", NodeEditor.Clear, skin.GetStyle("adjustBodyButton"));
            #endregion

            #region EditorType
            GUILayout.Label("---工作模式----", skin.GetStyle("adjustBodyLabel"));
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(NodeEditor.curNodeEditorState.IsStateMachine, new GUIContent("状态机型"), skin.toggle))
            {
                NodeEditor.curNodeEditorState.SelectedEditorType(EditorType.StateMachine,
                    ref NodeEditor.curNodeEditorState.IsStateMachine);
            }
            if (GUILayout.Toggle(NodeEditor.curNodeEditorState.IsTree, new GUIContent("树型"), skin.toggle))
            {
                NodeEditor.curNodeEditorState.SelectedEditorType(EditorType.StateMachine,
                    ref NodeEditor.curNodeEditorState.IsTree);
            }
            GUILayout.EndHorizontal();
            #endregion

            #region NodeType
            GUILayout.Label("---NodeType----", skin.GetStyle("adjustBodyLabel"));
            #endregion
        }
    }
}
