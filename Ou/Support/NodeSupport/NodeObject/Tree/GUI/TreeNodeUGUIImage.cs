using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false, "贴图设置", "Node")]
    public class TreeNodeUGUIImage : TreeNodeGUI
    {
        public override string GetId { get { return "贴图设置"; } }
        protected internal override void Evaluator()
        {
            var img = variables[0].obj as Image;
            var sprite = variables[1].obj as Sprite;
            img.sprite = sprite;
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            OuUIUtility.FormatLabel("UI目标");
            DrawFillsLayout(variables[0]);
            OuUIUtility.FormatLabel("贴图:");
            DrawFillsLayout(variables[1]);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUIImage>();
            node.Title = "贴图设置";
            node.rect = new Rect(pos, new Vector2(120, 180));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            node.CreateVariable();
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
            variables[0].setRangeType(this, "ImageUI");
            variables[1].setRangeType(this, "UISprite");
            base.Start();
        }
    }
}
