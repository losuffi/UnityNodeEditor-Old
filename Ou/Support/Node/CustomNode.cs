using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.Node
{
    [Node(false,"TestNode")]
    public class CustomNode:Node
    {
        protected override void Evaluator()
        {
        }

        protected internal override void NodeGUI()
        {
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<CustomNode>();

            node.Title = "Test!";
            node.rect = new Rect(pos, new Vector2(100, 80));
            return node;
        }

        private const string nodeId = "TestNode";
        public override string GetId { get { return nodeId; } }
    }
}
