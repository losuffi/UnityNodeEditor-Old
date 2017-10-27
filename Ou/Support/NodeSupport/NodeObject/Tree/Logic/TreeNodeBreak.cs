using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "支路中断", "Node")]
    public class TreeNodeBreak:TreeNodeLogic
    {
        public override string GetId { get { return "支路中断"; } }

        protected internal override void Evaluator()
        {
            base.Evaluator();
            state = TreeNodeResult.Break;
        }

        protected internal override void NodeGUI()
        {

        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeBreak>();
            node.Title = "支路中断";
            node.rect = new Rect(pos, new Vector2(60, 60));
            node.CreateNodeInput("PreIn", "工作状态");
            return node;

        }

        protected internal override TreeNodeResult OnUpdate()
        {
            return TreeNodeResult.Done;
        }

        protected internal override void OnStart()
        {
            base.OnStart();
        }

        protected internal override void Start()
        {
            base.Start();
        }
    }
}
