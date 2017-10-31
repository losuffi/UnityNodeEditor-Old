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
        [SerializeField] private string content = string.Empty;
        [SerializeField] private GlobalVariable ext;
        protected internal override void Evaluator()
        {            
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {

            GUILayout.Label("显示位置");
            GUILayout.Label("UI目标");
            ConnectionType.types["TextUI"].GUILayout(ref ext.obj);
            GUILayout.Label("内容文本:");
            OuUIUtility.FormatTextArea(ref content);

        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeGUIDialogue>();
            node.Title = "对话框";
            node.rect = new Rect(pos, new Vector2(120, 200));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
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
            var text = ext.obj as Text;
            var btn = text.gameObject.GetComponent<Button>();
            text.text = content;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(()=> { flag = true; });

        }

        protected internal override void Start()
        {
            ext = new GlobalVariable(typeof(Text), null, "TextUI", "text");
            base.Start();
        }
    }
}
