using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false,"对话框","Node")]
    public class TreeNodeGUIDialogue:TreeNodeGUI
    {
        public override string GetId { get { return "对话框"; } }
        private bool flag;
        protected internal override void Evaluator()
        {            
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {

            OuUIUtility.FormatLabel("显示位置");
            DrawFillsLayout(variables[0]);
            OuUIUtility.FormatLabel("内容文本:");
            DrawFillsLayout(variables[1]);
            OuUIUtility.FormatLabel("切换按钮");
            DrawFillsLayout(variables[2]);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeGUIDialogue>();
            node.Title = "对话框";
            node.rect = new Rect(pos, new Vector2(120, 200));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            node.CreateVariable();
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
            var text = variables[0].obj as Text;
            var btn = variables[2].obj as Button;
            text.text = variables[1].obj.ToString();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(()=> { flag = true; });

        }

        protected internal override void Start()
        {
            variables[0].setRangeType(this,"TextUI");
            variables[1].setRangeType(this,"字符串");
            variables[2].setRangeType(this,"ButtonUI");
            base.Start();
        }
    }
}
