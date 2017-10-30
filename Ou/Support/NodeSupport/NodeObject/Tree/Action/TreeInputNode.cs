using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Runtime
{
    [Node(false, "设置值","Node")]
    public class TreeInputNode : TreeNodeAction
    {
        [SerializeField]
        private GlobalVariable obj1;
        private int FillVariableTypeIndex1;

        [SerializeField] private GlobalVariable obj2;
        private int FillVariableTypeIndex2;

        private SettingType setType = SettingType.全局变量;
        private string nouseful;
        protected internal override void Evaluator()
        {
            base.Evaluator();
            if (curGraph.CheckKey(obj1.name, DataModel.Runtime))
            {
                curGraph.UpdateGlobalVarible(obj1.name, obj2.obj, DataModel.Runtime);
            }
        }

        protected internal override void Start()
        {
            base.Start();
            popupStructer = new PopupStructer(curGraph.selectorVariable("字符串", "真值", "实值"),curGraph);
            FillVariableTypeIndex1 = 0;
            FillVariableTypeIndex2 = 0;
            obj1 = new GlobalVariable(typeof(string), string.Empty, "字符串", "temporary");
            obj2 = new GlobalVariable(typeof(string), string.Empty, "字符串", "temporary");
            nouseful = string.Empty;
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("设置值");            
            if (popupStructer!=null)
            {
                OuUIUtility.FormatSelectedVariable_TypeFit(ref obj1, ref FillVariableTypeIndex1, popupStructer);
                OuUIUtility.FormatSetVariable_SelectedType(ref obj2, ref FillVariableTypeIndex2);
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
