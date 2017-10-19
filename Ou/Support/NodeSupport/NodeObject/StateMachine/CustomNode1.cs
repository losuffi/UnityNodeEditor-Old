using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.Node
{
    [Node(false, "状态机型|自定义|字符输入")]
    public class CustomNode1 : Node
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
            Node node = CreateInstance<CustomNode1>();

            node.Title = "字符串输入";
            node.rect = new Rect(pos, new Vector2(100, 80));
            node.CreateNodeOutput("Output 1", "String");
            return node;
        }
        private const string nodeId = "TestNode1";
        public override string GetId { get { return nodeId; } }
    }
}
