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
            nodeGraph = ScriptableObject.CreateInstance<NodeGraph>();
            nodeEditorState = ScriptableObject.CreateInstance<NodeEditorState>();
            nodeEditorState.CurGraph = nodeGraph;
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

                //TODO:Convert to Input System 
                nodeEditorState.CurGraphRect = ViewRect;
                if (e.button == 1 && e.type == EventType.mouseDown)
                {
                    menu.ShowAsContext();
                }
                //Draw NodeCanvas
                NodeEditor.DrawCanvas(nodeGraph, nodeEditorState);
                //if (e.button == 0 && e.type == EventType.MouseDrag)
                //{
                //    if (NodeEditor.curNodeEditorState != null)
                //    {
                //        NodeEditor.curNodeEditorState.UpdateData(e);
                //    }
                //}
            }
            GUILayout.EndArea();
        }

        void CallBack(object obj)
        {
            Node node = obj as Node;
            if (nodeGraph != null)
            {
                Vector2 pos = processE.mousePosition - ViewRect.position;
                nodeGraph.nodes.Add(Node.CreateNode(pos, node.GetId));
            }
        }
    }
}
