using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using UnityEngine;

namespace Ou.Support.Runtime
{
    [Node(false, "开始", "Node")]
    public class TreeStart:TreeNodeEvent
    {
        protected internal override void Evaluator()
        {
        }

        protected internal override void NodeGUI()
        {
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeStart>();
            node.Title = "开始";
            node.rect = new Rect(pos, new Vector2(60, 60));
            node.CreateNodeOutput("Output 1", "String");
            return node;
        }

        protected internal override TreeNodeResult OnUpdate()
        {
            return TreeNodeResult.Running;
        }

        protected internal override void OnStart()
        {
        }
    }
}
