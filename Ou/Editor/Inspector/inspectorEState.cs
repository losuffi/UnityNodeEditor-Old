using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Editor.Windows;
using Ou.Support.NodeSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Inspector
{
    [CustomEditor(typeof(NodeEditorState))]
    [CanEditMultipleObjects]
    public class inspectorEState:UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            NodeEditorState state = (NodeEditorState) target;
            if (GUILayout.Button("打开"))
            {

                string path = AssetDatabase.GetAssetPath(state);
                TriggerEditorWindows.Init();
                NodeEditor.LoadCanvas(path);
            }
        }
    }
    [CustomEditor(typeof(NodeGraph))]
    [CanEditMultipleObjects]
    public class inspectorEGraph : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            NodeGraph graph = (NodeGraph)target;
            if (GUILayout.Button("打开"))
            {
                string path = AssetDatabase.GetAssetPath(graph);
                TriggerEditorWindows.Init();
                NodeEditor.LoadCanvas(path);
            }
        }
    }
}
