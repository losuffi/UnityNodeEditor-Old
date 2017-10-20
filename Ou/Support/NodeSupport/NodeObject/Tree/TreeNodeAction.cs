using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ou.Support.NodeSupport
{
    [Node(false, "动作", "NodeType")]
    public class TreeNodeAction:TreeNode
    {
        private const string nodeId = "动作";
        public override string GetId { get { return nodeId; } }
    }
}
