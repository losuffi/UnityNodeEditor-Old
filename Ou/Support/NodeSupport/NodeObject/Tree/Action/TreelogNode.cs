using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace Ou.Support.NodeSupport
{
    [Node(false, "日志打印","Node")]
    public class TreelogNode:TreeNodeAction
    {
        protected internal override void Evaluator()
        {
            base.Evaluator();
            if (curGraph.CheckKey(variables[0].name))
            {
                Debug.Log(curGraph.ReadGlobalVariable(variables[0].name).obj);
            }
            else
            {
                Debug.Log(variables[0].obj);
            }
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label(variables[0].structerTypeRange.typeRange.Length.ToString());
            DrawFillsLayout(variables[0]);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreelogNode>();
            node.Title = "打印";
            node.rect = new Rect(pos, new Vector2(100, 80));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            return node;
        }

        private const string nodeId = "日志打印";
        protected internal override TreeNodeResult OnUpdate()
        {
            return TreeNodeResult.Done;
        }

        protected internal override void Start()
        {
            base.Start();
            variables[0].setRangeType(this);
        }

        protected internal override void OnStart()
        {
        }

        public override string GetId { get { return nodeId; } }
    }
}
