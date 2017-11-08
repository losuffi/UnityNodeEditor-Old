using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Serializable]
    public  class NodeGraph: ScriptableObject
    {
        //TODO:NodeCanvas Configue
        public List<Node> nodes=new List<Node>();

        [SerializeField]
        protected internal List<GlobalVariable> globalVariables =new List<GlobalVariable>();
        private List<GlobalVariable> globalVariablesRuntime=new List<GlobalVariable>();


        #region Node

       
        public void Clear()
        {
            if (nodes.Any())
            {
                foreach (Node node in nodes)
                {
                    foreach (NodeKnob nodeKnob in node.Knobs)
                    {
                        DestroyImmediate(nodeKnob, true);
                    }
                    DestroyImmediate(node, true);
                }
                nodes.Clear();
            }
        }

        public void Remove(Node node)
        {
            if (!nodes.Contains(node))
            {
                return;
            }
            nodes.Remove(node);
            node.RemoveLink(typeof(NodeOutput));
            node.RemoveLink(typeof(NodeInput));
            foreach (NodeKnob nodeKnob in node.Knobs)
            {
                DestroyImmediate(nodeKnob, true);
            }
            DestroyImmediate(node,true);
        }

        public Node CheckFocus(Vector2 pos)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].nodeRect.Contains(pos))
                {
                    return nodes[i];
                }
            }
            return null;
        }

        public NodeKnob CheckFocusKnob(Vector2 pos)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < nodes[i].Knobs.Count; j++)
                {
                    if (nodes[i].Knobs[j].rect.Contains(pos))
                    {
                        return nodes[i].Knobs[j];
                    }
                }
            }
            return null;
        }

        public void AddNode(Node prototypeNode,Vector2 pos)
        {
            Undo.RecordObject(this, "AddNode");
            Node node = Node.CreateNode(pos, prototypeNode.GetId);
            nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            foreach (NodeInput input in node.inputKnobs)
            {
                AssetDatabase.AddObjectToAsset(input, node);
            }
            foreach (NodeOutput nodeOutputKnob in node.outputKnobs)
            {
                AssetDatabase.AddObjectToAsset(nodeOutputKnob, node);
            }
            node.curGraph = this;
            node.Start();
        }


        public void InitNode(Node node,Vector2 pos)
        {
            node = node.Create(
                (pos - NodeEditor.curNodeEditorState.PanOffset) / NodeEditor.curNodeEditorState.GraphZoom);
            node.Init();
            nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            foreach (NodeKnob c in node.Knobs)
            {
                if (c.GetType() == typeof(NodeInput))
                {
                    var input = c as NodeInput;
                    AssetDatabase.AddObjectToAsset(input, node);
                }
                else
                {
                    var output = c as NodeOutput;
                    AssetDatabase.AddObjectToAsset(output, node);
                }                   
            }
            node.curGraph = this;
            node.Start();
        }


        #endregion




        #region Variable

        public void AddGlobalVariable(GlobalVariable globalVariable)
        {
            globalVariables.Add(globalVariable);
        }
        public string[] selectorVariable(params string[] id)
        {
            List<string> res=new List<string>();
            for (int j = 0; j < globalVariables.Count; j++)
            {
                foreach (string s in id)
                {
                    if (globalVariables[j].identity.Equals(s))
                    {
                        res.Add(globalVariables[j].name);
                        break;
                    }
                }
            }
            return res.ToArray();
        }

        public GlobalVariable ReadGlobalVariable(string key)
        {
            if (CheckKey(key))
            {
                var tar = globalVariables.Find(z => z.name.Equals(key));
                return tar;
            }
            return new GlobalVariable(typeof(string), string.Empty, "字符串", "none");
        }
        public GlobalVariable ReadGlobalVariable(int index)
        {
            if (globalVariables.Count>index)
            {
                var tar = globalVariables[index];
                return tar;
            }
            return new GlobalVariable(typeof(string), string.Empty, "字符串", "none");
        }
        public void UpdateGlobalVarible(string key,object obj)
        {
            if (CheckKey(key))
            {
                var tar = globalVariables.Find(z => z.name.Equals(key));
                tar.obj = obj;
                tar.ConvertString();
            }
        }
        public void RemoveGlobalVariable(int index)
        {
            globalVariables.RemoveAt(index);
        }
        public bool CheckKey(string key)
        {
            for (int i = 0; i < globalVariables.Count; i++)
            {
                if (globalVariables[i].name.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public void VariableTypeCheck(ref GlobalVariable obj)
        {
            obj = obj.isFromGlobaldatas ? ReadGlobalVariable(obj.name) : obj;
        }

        public int GlobalVariablesCount
        {
            get { return globalVariables.Count; }
        }

        public void InitialzationVariable()
        {
            globalVariablesRuntime.Clear();
            for (int gvCount = 0; gvCount < globalVariables.Count; gvCount++)
            {
                var tar = new GlobalVariable(globalVariables[gvCount]);
                tar.ConvertObject();
                globalVariablesRuntime.Add(tar);
            }
        }
        public void EndRuntimeVariable()
        {
            if (!globalVariablesRuntime.Any())
            {
                return;
            }
            globalVariables.Clear();
            for (int gvCount = 0; gvCount < globalVariablesRuntime.Count; gvCount++)
            {
                globalVariables.Add(globalVariablesRuntime[gvCount]);
            }
            globalVariablesRuntime.Clear();
        }

        #endregion
        //Work :需要搭建， Test Vision
    }
}
