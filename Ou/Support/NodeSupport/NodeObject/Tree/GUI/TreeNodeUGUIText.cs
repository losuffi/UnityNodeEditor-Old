using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false, "文本框设置", "Node")]
    public class TreeNodeUGUIText : TreeNodeGUI
    {
        public override string GetId { get { return "文本框设置"; } }
       // [SerializeField] private string content = string.Empty;
        protected internal override void Evaluator()
        {
            var text = variables[0].obj as Text;
            text.text = variables[1].obj.ToString();
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("显示位置");
            GUILayout.Label("UI目标");
           // ConnectionType.types["TextUI"].GUILayout(ref ext.obj);
            DrawFillsLayout(variables[0]);
            GUILayout.Label("内容文本:");
            DrawFillsLayout(variables[1]);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUIText>();
            node.Title = "文本框设置";
            node.rect = new Rect(pos, new Vector2(150, 240));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable(typeof(Text), null, "TextUI", "text");
            variables[0].setRangeType(node,"TextUI");
            node.CreateVariable();
            variables[1].setRangeType(node, "字符串");
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
          //  ext = new GlobalVariable(typeof(Text), null, "TextUI", "text");
            base.Start();
        }
    }
}
