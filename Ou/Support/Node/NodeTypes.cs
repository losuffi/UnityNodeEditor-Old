using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Node
{
    public static class NodeTypes
    {
        public static Dictionary<Node, NodeData> nodes;
        public static Dictionary<EditorType?, GenericMenu> menus;
        public static Node getDefaultNode(string nodeName)
        {
            return nodes.Keys.Single(ar => ar.GetId == nodeName);
        }
        public static void FetchNode()
        {
            nodes=new Dictionary<Node, NodeData>();
            menus=new Dictionary<EditorType?, GenericMenu>();
            IEnumerable<Assembly> scriptAssemblies =
                AppDomain.CurrentDomain.GetAssemblies().Where(ar=>ar.FullName.Contains("Assembly-"));
            foreach (var assembly in scriptAssemblies)
            {
                foreach (Type type in assembly.GetTypes().Where(ar=>ar.IsClass&&!ar.IsAbstract&&ar.IsSubclassOf(typeof(Node))))
                {
                    Node node=ScriptableObject.CreateInstance(type.Name) as  Node;
                    object[] attrs = type.GetCustomAttributes(typeof(NodeAttribute), false);
                    NodeAttribute attr=attrs[0] as NodeAttribute;
                    if (attr == null||!attr.IsHide)
                    {
                        //if (menus.ContainsKey(attr.Type))
                        //{
                        //    menus[attr.Type].AddItem(new GUIContent(NodeTypes.nodes[node].Adress), false, CallBack,
                        //        node);
                        //}
                        nodes.Add(node,
                            new NodeData(attr == null ? node.name : attr.Context, attr == null ? null : attr.Type));
                    }
                }
            }
        }

        //public static void CallBack(object obj)
        //{
        //    Node node = obj as Node;
        //    if (NodeEditor.curNodeGraph != null)
        //    {
        //        Vector2 pos = processE.mousePosition - ViewRect.position;
        //        NodeEditor.curNodeGraph.AddNode(node, pos);
        //    }
        //}
    }

    public struct NodeData
    {
        public string Adress;
        public EditorType? Type;

        public NodeData(string adress,EditorType? type)
        {
            Adress = adress;
            Type = type;
        }
    }
    public class NodeAttribute : Attribute
    {
        public bool IsHide { get; private set; }
        public string Context { get; private set; }
        public EditorType? Type { get; private set; }
        public NodeAttribute(bool isHide, string context,EditorType type)
        {
            IsHide = isHide;
            Context = context;
            Type = type;
        }
    }
}
