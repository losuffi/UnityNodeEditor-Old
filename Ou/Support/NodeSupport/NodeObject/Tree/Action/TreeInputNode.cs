using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using UnityEngine;

namespace Ou.Support.Runtime
{
    [Node(false, "字符输入","Node")]
    public class TreeInputNode : TreeNodeAction
    {
        private string Value=string.Empty;

        protected internal override void Evaluator()
        {
            outputKnobs[0].SetValue<string>(Value);
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("输入");
            Value = GUILayout.TextField(Value);
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeInputNode>();
            node.Title = "字符串输入";
            node.rect = new Rect(pos, new Vector2(100, 80));
            node.CreateNodeOutput("Output 1", "String");
            return node;
        }
        private const string nodeId = "TestNode1";
        public override string GetId { get { return nodeId; } }
    }
}
