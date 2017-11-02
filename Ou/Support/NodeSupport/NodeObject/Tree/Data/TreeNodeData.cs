using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "数据", "NodeType")]
    public class TreeNodeData : TreeNode
    {
        private const string nodeId = "数据";
        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            base.NodeGUI();
        }

        public override Node Create(Vector2 pos)
        {
            return base.Create(pos);
        }

        protected internal override TreeNodeResult OnUpdate()
        {
            return base.OnUpdate();
        }

        protected internal override void OnStart()
        {
            base.OnStart();
        }

        public override string GetId { get { return nodeId; } }
    }
}
