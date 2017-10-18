using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Ou.Support.OuUtility;
using UnityEditor;
namespace Ou.Support.Node
{
    public abstract class Node:ScriptableObject
    {
        protected GUISkin NodeSkin;
        public string Title;
        public Rect rect=new Rect();
        public Rect nodeRect;

        protected abstract void Evaluator();
        [NonSerialized] public List<NodeKnob> Knobs = new List<NodeKnob>();
        [SerializeField]
        internal List<NodeInput> inputKnobs=new List<NodeInput>();
        [SerializeField]
        internal List<NodeOutput> outputKnobs=new List<NodeOutput>();


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
                TriggerEditorUtility.Init();
            }
            node = node.Create(
                (pos - NodeEditor.curNodeEditorState.PanOffset) / NodeEditor.curNodeEditorState.GraphZoom);
            node.Init();
            return node;
        }
        protected internal virtual void Draw()
        {
            //TODO:DrawNode
            nodeRect = rect;
            NodeEditor.RectConverting(ref nodeRect);
            Vector2 contentOffset = new Vector2(0, 20);
            Rect nodeHead = new Rect(nodeRect.x, nodeRect.y, nodeRect.width, contentOffset.y);
            Rect nodeBody = new Rect(nodeRect.x, nodeRect.y + contentOffset.y, nodeRect.width,
                nodeRect.height - contentOffset.y);
            GUI.Label(nodeHead, Title,
                NodeEditor.curNodeEditorState.SelectedNode == this
                    ? NodeSkin.GetStyle("nodeHeadSelected")
                    : NodeSkin.GetStyle("nodeHead"));
            GUI.BeginGroup(nodeBody);
            nodeBody.position = Vector2.zero;
            GUILayout.BeginArea(nodeBody, NodeEditor.curNodeEditorState.SelectedNode == this
                ? NodeSkin.GetStyle("nodeBodySelected")
                : NodeSkin.GetStyle("nodeBody"));
            NodeGUI();
            GUILayout.EndArea();
            GUI.EndGroup();
            DrawKnob();
            DrawConnections();
        }

        protected internal virtual void DrawKnob()
        {
            CheckKnobMigration();
            foreach (var input in inputKnobs)
            {
                input.Draw();
            }
            foreach (var output in outputKnobs)
            {
                output.Draw();
            }
        }

        protected internal virtual void DrawConnections()
        {
            CheckKnobMigration();
            foreach (NodeInput input in inputKnobs)
            {
                input.DrawConnections();
            }
        }

        internal void CheckKnobMigration()
        {
            if (Knobs.Count == 0 && inputKnobs.Count + outputKnobs.Count != 0)
            {
                Knobs.AddRange(inputKnobs.Cast<NodeKnob>());
                Knobs.AddRange(outputKnobs.Cast<NodeKnob>());
            }
        }

        protected internal abstract void NodeGUI();


        public abstract Node Create(Vector2 pos);

        public abstract string GetId { get; }

        #region Knops
        public NodeInput CreateNodeInput(string inputName, string inputType)
        {
            return NodeInput.Create(this, inputName, inputType);
        }
        public NodeInput CreateNodeInput(string inputName, string inputType,Side sd)
        {
            return NodeInput.Create(this, inputName, inputType, sd);
        }
        public NodeInput CreateNodeInput(string inputName, string inputType,Side sd,float offset)
        {
            return NodeInput.Create(this, inputName, inputType, sd,offset);
        }
        public NodeOutput CreateNodeOutput(string outputName, string outputType)
        {
            return NodeOutput.Create(this, outputName, outputType, Side.Right);
        }
        public NodeOutput CreateNodeOutput(string outputName, string outputType, Side sd)
        {
            return NodeOutput.Create(this, outputName, outputType,sd);
        }
        public NodeOutput CreateNodeOutput(string outputName, string outputType, Side sd, float offset)
        {
            return NodeOutput.Create(this, outputName, outputType, sd, offset);
        }
        #endregion
    }
}
