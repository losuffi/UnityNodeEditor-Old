using Ou.Support.Node;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    public class TriggerEditorCanvasView:ViewBase
    {
        private GenericMenu menu;
        private Event processE;

        private NodeGraph nodeGraph;
        private NodeEditorState nodeEditorState;

        public TriggerEditorCanvasView(string title) : base(title)
        {
            NodeEditor.InitAssetData(out nodeEditorState, out nodeGraph);
            menu = new GenericMenu();
            foreach (Node node in NodeTypes.nodes.Keys)
            {
                menu.AddItem(new GUIContent(NodeTypes.nodes[node].Adress), false, CallBack, node);
            }
        }
        public override void ProcessEvent(Event e)
        {
            base.ProcessEvent(e);
            processE = e;
        }

        public override void UpdateView(Rect size, Rect percentageSize, Event e)
        {
            base.UpdateView(size, percentageSize, e);
            ProcessEvent(e);
            GUI.Box(ViewRect, Title, ViewSkin.GetStyle("TriggerEditorCanvas"));
            GUILayout.BeginArea(ViewRect);
            {
                nodeEditorState.CurGraphRect = ViewRect;
                if (e.button == 1 && e.type == EventType.mouseDown)
                {
                    menu.ShowAsContext();
                }
                NodeEditor.DrawCanvas(nodeGraph, nodeEditorState);
            }
            GUILayout.EndArea();
        }

        void CallBack(object obj)
        {
            Node node = obj as Node;
            if (nodeGraph != null)
            {
                Vector2 pos = processE.mousePosition - ViewRect.position;
                nodeGraph.AddNode(node, pos);
            }
        }
    }
}
