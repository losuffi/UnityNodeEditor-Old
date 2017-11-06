using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "等待时间", "Node")]
    public class TreeNodeActWait : TreeNodeAction
    {
        private float initTime;
        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            DrawFillsLayout(variables[0]);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeActWait>();
            node.Title = "等待时间";
            node.rect = new Rect(pos, new Vector2(100, 100));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            return node;
        }

        private const string nodeId = "等待时间";
        protected internal override TreeNodeResult OnUpdate()
        {
            if (Time.time - initTime > (float) variables[0].obj)
            {
                return TreeNodeResult.Done;
            }
            return TreeNodeResult.Running;
        }

        protected internal override void Start()
        {
            base.Start();
            variables[0].setRangeType(this,"实值");
        }

        protected internal override void OnStart()
        {
            initTime = Time.time;
        }

        public override string GetId { get { return nodeId; } }
    }
}
