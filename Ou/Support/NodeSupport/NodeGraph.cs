using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Node
{
    [Serializable]
    public  class NodeGraph: ScriptableObject
    {
        //TODO:NodeCanvas Configue
        public List<Node> nodes=new List<Node>();

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
        }
        //Work :需要搭建， Test Vision；
        public void Run()
        {
            foreach (Node node in nodes)
            {
                node.Evaluator();
            }
        }
    }
}
