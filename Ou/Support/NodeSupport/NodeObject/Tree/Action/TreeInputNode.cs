using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Runtime
{
    [Node(false, "字符输入","Node")]
    public class TreeInputNode : TreeNodeAction
    {
        private string fillValue = string.Empty;
        private string Value=string.Empty;
        private SettingType setType = SettingType.全局变量;
        protected internal override void Evaluator()
        {
            base.Evaluator();
            if (curGraph.CheckKey(popupStructer.value))
            {
                curGraph.UpdateGlobalVarible(popupStructer.value, Value);
            }
        }

        protected internal override void Start()
        {
            base.Start();
            popupStructer = new PopupStructer(curGraph.selectorVariable("字符串"));
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("设置值");
            Value = GUILayout.TextField(Value);
            setType = (SettingType)EditorGUILayout.EnumPopup(setType);
            if (!popupStructer.Equals(default(PopupStructer)))
            {
                OuUIUtility.FormatSetType(setType, ref fillValue, ref popupStructer);
            }
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeInputNode>();
            node.Title = "字符串输入";
            node.rect = new Rect(pos, new Vector2(100,120));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
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
