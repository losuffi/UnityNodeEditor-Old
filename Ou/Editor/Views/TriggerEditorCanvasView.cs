using Ou.Support.Node;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    public class TriggerEditorCanvasView:ViewBase
    {
        private GenericMenu menu;
        private Event processE;
        public TriggerEditorCanvasView(string title) : base(title)
        {
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
                //TODO:ContexView
                if (e.button == 1 && e.type == EventType.mouseDown)
                {
                    menu.ShowAsContext();
                }
            }
            GUILayout.EndArea();
        }

        void CallBack(object obj)
        {
            Node node = obj as Node;
            if (NodeEditor.curNodeGraph != null)
            {
                NodeEditor.curNodeGraph.nodes.Add(Node.CreateNode(processE.mousePosition, node.GetId));
            }
            //Node.CreateNode(e.mousePosition, "TestNode");
        }
    }
}
