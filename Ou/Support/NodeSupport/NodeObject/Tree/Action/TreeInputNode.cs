using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "设置值","Node")]
    public class TreeInputNode : TreeNodeAction
    {
        protected internal override void Evaluator()
        {
            base.Evaluator();
            if (curGraph.CheckKey(variables[0].name))
            {
                curGraph.UpdateGlobalVarible(variables[0].name, variables[1].obj);
            }
        }

        protected internal override void Start()
        {
            base.Start();
            variables[0].setRangeType(this, "字符串", "真值", "实值");
            variables[1].setRangeType(this, "字符串", "真值", "实值");
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("设置值");            
            DrawFillsLayout(variables[0]);
            DrawFillsLayout(variables[1]);
        }


        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeInputNode>();
            node.Title = "字符串输入";
            node.rect = new Rect(pos, new Vector2(100,120));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();         
            node.CreateVariable();

            return node;
        }
        private const string nodeId = "TestNode1";
        protected internal override TreeNodeResult OnUpdate()
        {
            return TreeNodeResult.Done;
        }

        protected internal override void OnStart()
        {
        }

        public override string GetId { get { return nodeId; } }
    }
}
