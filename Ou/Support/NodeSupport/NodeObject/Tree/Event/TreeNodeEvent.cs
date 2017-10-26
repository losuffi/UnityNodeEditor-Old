using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "事件", "NodeType")]
    public class TreeNodeEvent:TreeNode
    {
        private const string nodeId = "事件";
        protected internal override void Evaluator()
        {
            throw new NotImplementedException();
        }

        protected internal override void NodeGUI()
        {
            throw new NotImplementedException();
        }

        public override Node Create(Vector2 pos)
        {
            throw new NotImplementedException();
        }

        protected internal override TreeNodeResult OnUpdate()
        {
            throw new NotImplementedException();
        }

        protected internal override void OnStart()
        {
            throw new NotImplementedException();
        }

        public override string GetId { get { return nodeId; } }
    }
}
