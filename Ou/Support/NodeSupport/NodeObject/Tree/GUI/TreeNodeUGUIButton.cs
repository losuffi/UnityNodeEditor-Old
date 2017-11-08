using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false, "UI按钮点击等待", "Node")]
    public class TreeNodeUGUIButton : TreeNodeGUI
    {
        public override string GetId { get { return "UI按钮点击等待"; } }
        private bool flag;
        private UnityAction tar;
        protected internal override void Evaluator()
        {
            base.Evaluator();
            var btn = variables[0].obj as Button;
            btn.onClick.RemoveListener(tar);
        }

        protected internal override void NodeGUI()
        {
            OuUIUtility.FormatLabel("UI目标");
            DrawFillsLayout(variables[0]);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUIButton>();
            node.Title = "点击等待";
            node.rect = new Rect(pos, new Vector2(100, 100));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            return node;

        }

        protected internal override TreeNodeResult OnUpdate()
        {
            if (flag)
                return TreeNodeResult.Done;
            return TreeNodeResult.Running;
        }

        protected internal override void OnStart()
        {
            base.OnStart();
            flag = false;
            var btn = variables[0].obj as Button;
            tar = () => { flag = true; };
            btn.onClick.AddListener(tar);
        }

        protected internal override void Start()
        {
            variables[0].setRangeType(this, "ButtonUI");
            base.Start();
        }
    }
}
