using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.Runtime;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"判断相等","Node")]
    public class TreeNodeEquls:TreeNodeLogic
    {
        private const string nodeId = "判断相等";
        private bool result;
        private SettingType setType1 = SettingType.填充;
        private SettingType setType2 = SettingType.全局变量;

        private int FillVariableTypeIndex1;
        [SerializeField]
        private GlobalVariable obj1;

        private int FillVariableTypeIndex2;
        [SerializeField]
        private GlobalVariable obj2;

        private string Nonuseful;
        public override string GetId { get { return nodeId; } }
        protected internal override void Evaluator()
        {
            if (result)
            {
                Goto(0);
            }
            else
            {
                Goto(1);
            }
        }

        protected internal override void NodeGUI()
        {
            if (popupStructer!=null)
            {
                setType1 = (SettingType)EditorGUILayout.EnumPopup(setType1);
                if (setType1 == SettingType.填充)
                {
                    OuUIUtility.FormatSetVariable_SelectedType(ref obj1, ref FillVariableTypeIndex1);
                }
                else
                {
                    OuUIUtility.FormatSelectedVariable_TypeFit(ref obj1, ref FillVariableTypeIndex1,popupStructer);
                }
                setType2 = (SettingType)EditorGUILayout.EnumPopup(setType2);
                if (setType2 == SettingType.填充)
                {
                    OuUIUtility.FormatSetVariable_SelectedType(ref obj2, ref FillVariableTypeIndex2);
                }
                else
                {
                    OuUIUtility.FormatSelectedVariable_TypeFit(ref obj2, ref FillVariableTypeIndex2, popupStructer);
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("True",NodeSkin.GetStyle("TreeNodeConditionLabel"));
            GUILayout.Label("False", NodeSkin.GetStyle("TreeNodeConditionLabel"));
            GUILayout.EndHorizontal();
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeEquls>();
            node.Title = "是否相等";
            node.rect = new Rect(pos, new Vector2(100, 180));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态", Side.Bottom);
            node.CreateNodeOutput("Nextout", "工作状态", Side.Bottom, 50);
            return node;
        }

        protected internal override TreeNodeResult OnUpdate()
        {
            curGraph.VariableTypeCheck(ref obj1, DataModel.Runtime);
            curGraph.VariableTypeCheck(ref obj2, DataModel.Runtime);
            if (obj1.type != obj2.type)
            {
                result = false;
            }
            else
            {
                result = obj1.obj.Equals(obj2.obj);
            }
            return TreeNodeResult.Done;
        }

        protected internal override void OnStart()
        {
            base.OnStart();
        }

        protected internal override void Start()
        {
            base.Start();
            popupStructer=new PopupStructer(curGraph.selectorVariable("字符串", "实值", "真值"),curGraph);
            result = false;
            FillVariableTypeIndex1 = 0;
            obj1 = new GlobalVariable(typeof(string), string.Empty, "字符串", "temporary");
            FillVariableTypeIndex2 = 0;
            obj2 = new GlobalVariable(typeof(string), string.Empty, "字符串", "temporary");
            Nonuseful = string.Empty;
        }
    }
}
