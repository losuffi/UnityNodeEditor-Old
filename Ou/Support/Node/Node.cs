using UnityEngine;
using System;
using Ou.Support.OuUtility;
using UnityEditor;
namespace Ou.Support.Node
{
    public abstract class Node:ScriptableObject
    {
        protected GUISkin NodeSkin;
        public string Title;
        public Rect rect=new Rect();
        public EditorWindow editorwindow;
        protected abstract void Evaluator();
        public virtual void Init()
        {
            NodeSkin = OuUIUtility.GetGUISkinStyle("NormalSkin");
        }

        public static Node CreateNode(Vector2 pos,string nodeId)
        {
            Node node = NodeTypes.getDefaultNode(nodeId);
            if (node == null)
            {
                Debug.Log("node editor running is warning!");
            }
            node = node.Create(pos);
            node.Init();
            return node;
        }
        protected internal virtual void Draw()
        {
            //TODO:DrawNode
            Rect nodeRect = rect;
            //nodeRect.position = NodeEditor.curNodeEditorState.PanAdjust + NodeEditor.curNodeEditorState.PanOffset;
            //nodeRect.position = new Vector2(0, 0);
            Vector2 contentOffset = new Vector2(0, 20);
            Rect nodeHead = new Rect(nodeRect.x, nodeRect.y, nodeRect.width, contentOffset.y);
            Rect nodeBody = new Rect(nodeRect.x, nodeRect.y + contentOffset.y, nodeRect.width,
                nodeRect.height - contentOffset.y);
            GUI.Label(nodeHead, Title, NodeSkin.GetStyle("nodeHead"));
            GUI.BeginGroup(nodeBody);
            nodeBody.position = Vector2.zero;
            GUILayout.BeginArea(nodeBody, NodeSkin.GetStyle("nodeBody"));
            NodeGUI();
            GUILayout.EndArea();
            GUI.EndGroup();
        }

        protected internal abstract void NodeGUI();


        public abstract Node Create(Vector2 pos);

        public abstract string GetId { get; }
    }
}
