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
        #region NodeType
        public static Dictionary<Node, NodeData> nodes;
        public static Node getDefaultNode(string nodeName)
        {
            return nodes.Keys.Single(ar => ar.GetId == nodeName);
        }
        public static void FetchNode()
        {
            nodes=new Dictionary<Node, NodeData>();
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
                        nodes.Add(node,
                            new NodeData(attr == null ? node.name : attr.Context));
                    }
                }
            }
        }
        #endregion

        #region TypeStructSetting

        #endregion
    }


    public struct NodeData
    {
        public string Adress;
        public bool ToggleState;

        public NodeData(string adress)
        {
            Adress = adress;
            ToggleState = false;
        }
    }
    public class NodeAttribute : Attribute
    {
        public bool IsHide { get; private set; }
        public string Context { get; private set; }
        public NodeAttribute(bool isHide, string context)
        {
            IsHide = isHide;
            Context = context;
        }
    }
}
