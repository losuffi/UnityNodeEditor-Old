using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ou.Support.UnitSupport;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "反射", "Node")]
    public class TreeNodeEvent_Reflection : TreeNodeEvent
    {
        [SerializeField] private int index;
        private Delegate method;
        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            OuUIUtility.FormatLabel("参数：");
            DrawFillsLayout(variables[0]);
            OuUIUtility.FormatLabel("反射方法：");
            OuUIUtility.FormatPopup(ref index, TreeNodeReflectionSystem.methodNames);
            base.NodeGUI();
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeEvent_Reflection>();
            node.Title = "反射";
            node.rect = new Rect(pos, new Vector2(120, 200));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            return node;
        }

        protected internal override TreeNodeResult OnUpdate()
        {
            bool res = (bool) method.DynamicInvoke(variables[0]);
            if (res)
            {
                return TreeNodeResult.Done;
            }
            return TreeNodeResult.Running;
        }

        protected internal override void OnStart()
        {
            method = TreeNodeReflectionSystem.GetMethod(TreeNodeReflectionSystem.methodNames[index]);
            base.OnStart();
        }

        public override string GetId { get { return "反射"; } }
        protected internal override void Start()
        {
            variables[0].setRangeType(this,"Unit");
            base.Start();

        }
    }
}
