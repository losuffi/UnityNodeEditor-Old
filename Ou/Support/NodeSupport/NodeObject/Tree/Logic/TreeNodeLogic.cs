using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "逻辑", "NodeType")]
    public class TreeNodeLogic : TreeNode
    {
        private const string nodeId = "逻辑";
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
