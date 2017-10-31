using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"循环-A类","Node")]
    public class TreeNodeItera:TreeNodeLogic
    {
        public override string GetId { get { return "循环-A类"; } }

        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {

        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeItera>();
            node.Title = "循环A";
            node.rect = new Rect(pos, new Vector2(100, 80));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateNodeOutput("Forout", "工作状态",Side.Right);
            return node;

        }

        protected internal override TreeNodeResult OnUpdate()
        {
            var res = feedback;
            Evaluator();
            if (res == TreeNodeResult.Break)
            {
                feedback = TreeNodeResult.Done;
                FeedBack();
                return TreeNodeResult.Done;
            }
            else if(res == TreeNodeResult.Running||res== TreeNodeResult.Idle)
            {
                return TreeNodeResult.Running;
            }else if (res == TreeNodeResult.Done)
            {
                StateReset("Forout");
                Goto(GotoType.Single, "Forout");
                return TreeNodeResult.Running;
            }
            else
            {
                return TreeNodeResult.Failed;
            }
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
