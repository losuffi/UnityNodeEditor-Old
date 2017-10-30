using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "算术计算", "Node")]
    public class TreeNodeCalculation:TreeNodeMath
    {
        private enum CalcuType
        {
            加,
            减,
            乘,
            除,
        }

        public override string GetId { get { return "算术计算"; } }

        private string Nonuseful;

        private SettingType setType1 = SettingType.全局变量;
        private SettingType setType2 = SettingType.填充;
        private SettingType setType3 = SettingType.填充;
        private CalcuType cType=CalcuType.加;

        [SerializeField]
        private GlobalVariable obj1;
        private int FillVariableTypeIndex1;

        private int FillVariableTypeIndex2;
        [SerializeField]
        private GlobalVariable obj2;

        private int FillVariableTypeIndex3;
        [SerializeField]
        private GlobalVariable obj3;

        protected internal override void Evaluator()
        {
            base.Evaluator();
            obj1 = curGraph.ReadGlobalVariable(obj1.name, DataModel.Runtime);
            curGraph.VariableTypeCheck(ref obj2, DataModel.Runtime);
            curGraph.VariableTypeCheck(ref obj3, DataModel.Runtime);
            float val_1 = obj2.identity.Equals("真值") ? (int) obj2.obj * 1.0f : (float) obj2.obj;
            float val_2 = obj3.identity.Equals("真值") ? (int) obj3.obj * 1.0f : (float) obj3.obj;
            float res = obj1.identity.Equals("真值") ? (int) obj1.obj * 1.0f : (float) obj1.obj;
            switch (cType)
            {
                case CalcuType.加:
                    res = val_1 + val_2;
                    break;
                case CalcuType.减:
                    res = val_1 - val_2;
                    break;
                case CalcuType.乘:
                    res = val_1 * val_2;
                    break;
                case CalcuType.除:
                    res = val_1 / val_2;
                    break;
            }
            if (obj1.identity.Equals("真值"))
            {
                curGraph.UpdateGlobalVarible(obj1.name, Mathf.RoundToInt(res), DataModel.Runtime);

            }
            else
            {
                curGraph.UpdateGlobalVarible(obj1.name, res, DataModel.Runtime);
            }
        }


        protected internal override void NodeGUI()
        {

            if (popupStructer!=null)
            {
                GUILayout.Label("结果：");
                OuUIUtility.FormatSelectedVariable_TypeFit(ref obj1, ref FillVariableTypeIndex1, popupStructer);

                setType2 = (SettingType)EditorGUILayout.EnumPopup(setType2);
                if (setType2 == SettingType.填充)
                {
                    OuUIUtility.FormatSetVariable_SelectedType(ref obj2, ref FillVariableTypeIndex2);
                }
                else
                {
                    OuUIUtility.FormatSelectedVariable_TypeFit(ref obj2, ref FillVariableTypeIndex2, popupStructer);
                }
                cType = (CalcuType)EditorGUILayout.EnumPopup(cType);
                setType3 = (SettingType)EditorGUILayout.EnumPopup(setType3);
                if (setType3 == SettingType.填充)
                {
                    OuUIUtility.FormatSetVariable_SelectedType(ref obj3, ref FillVariableTypeIndex3);
                }
                else
                {
                    OuUIUtility.FormatSelectedVariable_TypeFit(ref obj3, ref FillVariableTypeIndex3, popupStructer);
                }
            }
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeCalculation>();
            node.Title = "算术计算";
            node.rect = new Rect(pos, new Vector2(100, 220));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
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
            base.Start();
            popupStructer = new PopupStructer(curGraph.selectorVariable("实值", "真值"), curGraph);
            FillVariableTypeIndex1 = 0;
            obj1 = new GlobalVariable(typeof(string), string.Empty, "字符串", "temporary");
            FillVariableTypeIndex2 = 0;
            obj2 = new GlobalVariable(typeof(string), string.Empty, "字符串", "temporary");
            FillVariableTypeIndex3 = 0;
            obj3 = new GlobalVariable(typeof(string), string.Empty, "字符串", "temporary");
            Nonuseful = string.Empty;
        }
    }
}
