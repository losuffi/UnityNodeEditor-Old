using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{ 
    [Node(false,"点击事件-A","Node")]
    public class TreeNodeClick:TreeNodeEvent
    {
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
            Node node = CreateInstance<TreeNodeClick>();
            node.Title = "点击事件";
            node.rect = new Rect(pos, new Vector2(60, 60));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            return node;
        }

        protected internal override TreeNodeResult OnUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                return TreeNodeResult.Done;
            }
            return TreeNodeResult.Running;
        }

        protected internal override void OnStart()
        {
            base.OnStart();
        }

        public override string GetId { get { return "点击事件-A"; } }
        protected internal override void Start()
        {
            base.Start();
        }
    }
}
