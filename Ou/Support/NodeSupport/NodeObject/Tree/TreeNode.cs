using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"树型","EditorType")]
    public class TreeNode:Node
    {
        protected internal override void Evaluator()
        {
                    
        }

        protected internal override void NodeGUI()
        {
        }

        public override Node Create(Vector2 pos)
        {
            return null;
        }

        private const string nodeId = "树型";
        public override string GetId { get { return nodeId; } }
    }
}
