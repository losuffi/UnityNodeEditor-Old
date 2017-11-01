using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"判断相等","Node")]
    public class TreeNodeEquls:TreeNodeLogic
    {
        private const string nodeId = "判断相等";
        private bool result;
        public override string GetId { get { return nodeId; } }
        protected internal override void Evaluator()
        {
            CallFeedBack();
            if (result)
            {
                Goto(0);
            }
            else
            {
                Goto(1);
            }
        }

        protected internal override void NodeGUI()
        {
            DrawFillsLayout(variables[0]);
            DrawFillsLayout(variables[1]);
            GUILayout.BeginHorizontal();
            GUILayout.Label("True",NodeSkin.GetStyle("TreeNodeConditionLabel"));
            GUILayout.Label("False", NodeSkin.GetStyle("TreeNodeConditionLabel"));
            GUILayout.EndHorizontal();
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeEquls>();
            node.Title = "是否相等";
            node.rect = new Rect(pos, new Vector2(100, 180));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态", Side.Bottom);
            node.CreateNodeOutput("Nextout", "工作状态", Side.Bottom, 50);
            node.CreateVariable();
            node.CreateVariable();
            return node;
        }

        protected internal override TreeNodeResult OnUpdate()
        {
            var obj1 = variables[0];
            var obj2 = variables[1];
            curGraph.VariableTypeCheck(ref obj1, DataModel.Runtime);
            curGraph.VariableTypeCheck(ref obj2, DataModel.Runtime);
            if (obj1.type != obj2.type)
            {
                result = false;
            }
            else
            {
                result = obj1.obj.Equals(obj2.obj);
            }
            return TreeNodeResult.Done;
        }

        protected internal override void OnStart()
        {
            base.OnStart();
        }

        protected internal override void Start()
        {
            base.Start();
            variables[0].setRangeType(this, "真值", "实值");
            variables[1].setRangeType(this, "真值", "实值");
        }
    }
}
