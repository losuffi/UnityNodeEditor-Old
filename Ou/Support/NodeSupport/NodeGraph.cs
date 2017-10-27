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
        public List<GlobalVariable> globalVariables=new List<GlobalVariable>();

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
                
                return globalVariables.Find(z => z.name.Equals(key));
            }
            return new GlobalVariable(typeof(string), string.Empty, "字符串", "none");
        }

        public void UpdateGlobalVarible(string key,object obj)
        {
            if (CheckKey(key))
            {
                var tar = globalVariables.Find(z => z.name.Equals(key));
                tar.obj = obj;
            }
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
        //Work :需要搭建， Test Vision
        public void TreeRun()
        {
            foreach (TreeNode node in nodes)
            {
                if(node.OnCheckCompelete())
                    continue;
                if (node.state == TreeNodeResult.Running)
                {
                    node.state = node.OnUpdate();
                }
                if (node.IsCompelete)
                {
                    node.Evaluator();
                }
            }
        }
    }
}
