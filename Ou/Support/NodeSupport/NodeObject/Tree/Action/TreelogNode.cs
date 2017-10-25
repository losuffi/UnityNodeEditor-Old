using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using UnityEditor;
using UnityEngine;
namespace Ou.Support.Runtime
{
    [Node(false, "日志打印","Node")]
    public class TreelogNode:TreeNodeAction
    {
        private string logString = string.Empty;
        private SettingType setType = SettingType.填充;

        protected internal override void Evaluator()
        {
            base.Evaluator();
            if (setType== SettingType.全局变量&& curGraph.CheckKey(popupStructer.value))
            {
                Debug.Log(curGraph.ReadGlobalVariable(popupStructer.value).obj.ToString());
            }
            else
            {
                Debug.Log(logString);
            }
        }

        protected internal override void NodeGUI()
        {
            setType = (SettingType)EditorGUILayout.EnumPopup(setType);
            if (!popupStructer.Equals(default(PopupStructer)))
            {
                OuUIUtility.FormatSetType(setType, ref logString, ref popupStructer);
            }
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreelogNode>();
            node.Title = "打印";
            node.rect = new Rect(pos, new Vector2(100, 80));
            node.CreateNodeInput("Input 1", "工作状态");
            node.CreateNodeOutput("Output 0", "工作状态");
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
            popupStructer = new PopupStructer(curGraph.selectorVariable("字符串", "实值", "真值"));
        }

        protected internal override void OnStart()
        {
        }

        public override string GetId { get { return nodeId; } }
    }
}
