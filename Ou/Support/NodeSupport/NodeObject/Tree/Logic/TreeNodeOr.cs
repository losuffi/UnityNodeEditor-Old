using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"或-A","Node")]
    public class TreeNodeOr:TreeNodeLogic
    {
        public override string GetId { get { return "或-A"; } }

        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {

        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeOr>();
            node.Title = "或A";
            node.rect = new Rect(pos, new Vector2(140, 50));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeInput("PreIn", "工作状态", Side.Top,40);
            node.CreateNodeInput("PreIn", "工作状态",Side.Top,80);
            node.CreateNodeOutput("Nextout", "工作状态", Side.Bottom,40);
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
