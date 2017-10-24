using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(true, "初始化", "Node")]
    public class TreeInitNode:TreeNode
    {
        protected internal override TreeNodeResult OnUpdate()
        {
            return TreeNodeResult.Done;
        }

        protected internal override void OnStart()
        {
        }

        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeInitNode>();
            node.Title = "初始";
            node.rect = new Rect(pos, new Vector2(60, 60));
            node.CreateNodeOutput("Output 1", "Workstate");
            return node;
        }

        public override string GetId { get{return "初始化";} }
    }
}
