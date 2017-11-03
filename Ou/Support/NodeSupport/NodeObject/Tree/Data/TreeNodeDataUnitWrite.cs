using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.UnitSupport;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "Unit数据写入", "Node")]
    public class TreeNodeDataUnitWrite : TreeNodeData
    {
        protected internal override void Evaluator()
        {
            base.Evaluator();
            UnitBase tar=variables[0].obj as UnitBase;
            if (tar.CheckKey(variables[1].name))
            {
                tar.UpdateGlobalVarible(variables[1].name, variables[2].obj);
            }
        }

        protected internal override void Start()
        {
            base.Start();
            variables[0].setRangeType(this, "Unit");
            variables[2].setRangeType(this);
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("设置值");
            DrawFillsLayout(variables[0]);
            if (variables[0].obj != null && variables[0].obj.GetType() == typeof(UnitBase))
            {
                UnitBase tar = variables[0].obj as UnitBase;
                variables[1].setRangeType(tar);
                DrawUnitHandle();
            }
            //   Debug.Log(Selection.activeObject.GetType());
        }

        void DrawUnitHandle()
        {
            GUILayout.Label("写入值：");
            DrawFillsLayout(variables[2]);
            GUILayout.Label("承接属性:");
            DrawUnitLayout(variables[1]);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeDataUnitWrite>();
            node.Title = "Unit数据写入";
            node.rect = new Rect(pos, new Vector2(120, 180));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            node.CreateVariable();
            node.CreateVariable();
            return node;
        }
        private const string nodeId = "Unit数据写入";
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
