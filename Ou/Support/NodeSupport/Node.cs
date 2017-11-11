using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
namespace Ou.Support.NodeSupport
{
    public abstract class Node:ScriptableObject
    {
        protected GUISkin NodeSkin;
        public string Title;
        public Rect rect=new Rect();
        public Rect nodeRect;
        public NodeGraph curGraph;
        protected internal abstract void Evaluator();
        [SerializeField] internal List<NodeKnob> Knobs = new List<NodeKnob>();
        [SerializeField]
        internal List<NodeInput> inputKnobs=new List<NodeInput>();
        [SerializeField]
        internal List<NodeOutput> outputKnobs=new List<NodeOutput>();

        public bool isNoneUsefulNode
        {
            get
            {
                foreach (NodeInput input in inputKnobs)
                {
                    if (input.connection != null)
                        return false;
                }
                return true;
            }
        }


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
            var npos = (pos - NodeEditor.curNodeEditorState.PanOffset) / NodeEditor.curNodeEditorState.GraphZoom;
            node = node.Create(
                npos);
            node.Init();
            return node;
        }
        protected internal virtual void Draw()
        {
            if (NodeSkin == null)
            {
                Init();
            }
            nodeRect = rect;
            NodeEditor.RectConverting(ref nodeRect);
            Vector2 contentOffset = new Vector2(0, 20);
            Rect nodeHead = new Rect(0, 0, nodeRect.width, contentOffset.y);
            Rect nodeBody = new Rect(nodeRect.x, nodeRect.y, nodeRect.width,
                nodeRect.height);
            GUI.BeginGroup(nodeBody);
            nodeBody.position = Vector2.zero;
            GUILayout.BeginArea(nodeBody, NodeEditor.curNodeEditorState.SelectedNode == this
                ? NodeSkin.GetStyle("nodeBodySelected")
                : NodeSkin.GetStyle("nodeBody"));
            GUI.Label(nodeHead,Title, NodeSkin.GetStyle("nodeHead"));
            GUILayout.Space(contentOffset.y);
            if (NodeEditor.curNodeEditorState.GraphZoom >= 1)
            {
                NodeGUI();
            }
            GUILayout.EndArea();
            GUI.EndGroup();
            DrawKnob();
            DrawConnections();
        }

        protected internal virtual void DrawKnob()
        {
            CheckKnobMigration();
            foreach (var res in Knobs)
            {
                res.Draw();
            }
        }

        protected internal void RemoveLink(Type type)
        {
            if (type == typeof(NodeOutput))
            {
                foreach (NodeOutput knob in outputKnobs)
                {
                    foreach (NodeInput knobConnection in knob.connections)
                    {
                        if (knobConnection != null)
                        {
                            knobConnection.connection = null;
                        }
                    }
                    knob.connections.Clear();
                }
            }
            else if(type==typeof(NodeInput))
            {
                foreach (NodeInput nodeInputKnob in inputKnobs)
                {
                    if (nodeInputKnob.connection != null)
                    {
                        nodeInputKnob.connection.connections.Remove(nodeInputKnob);
                        nodeInputKnob.connection = null;
                    }
                }
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

        protected internal virtual void Start()
        {
            
        }

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
            return NodeOutput.Create(this, outputName, outputType);
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
