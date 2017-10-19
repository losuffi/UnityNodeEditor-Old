using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.Node
{
    [Node(false, "树型|自定义|日志打印")]
    public class TreelogNode:Node
    {
        protected internal override void Evaluator()
        {
            Debug.Log(this.inputKnobs[0].GetValue<object>().ToString());
        }

        protected internal override void NodeGUI()
        {
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreelogNode>();

            node.Title = "打印";
            node.rect = new Rect(pos, new Vector2(100, 80));
            node.CreateNodeInput("Input 1", "Object");
            return node;
        }

        private const string nodeId = "日志打印";
        public override string GetId { get { return nodeId; } }
    }
}
