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
        [SerializeField]
        private CalcuType cType=CalcuType.加;

        protected internal override void Evaluator()
        {
            base.Evaluator();
            var obj2 = variables[1];
            var obj3 = variables[2];
            variables[0] = curGraph.ReadGlobalVariable(variables[0].name);
            curGraph.VariableTypeCheck(ref obj2);
            curGraph.VariableTypeCheck(ref obj3);
            float val_1 = obj2.identity.Equals("真值") ? (int) obj2.obj * 1.0f : (float) obj2.obj;
            float val_2 = obj3.identity.Equals("真值") ? (int) obj3.obj * 1.0f : (float) obj3.obj;
            float res = variables[1].identity.Equals("真值") ? (int)variables[1].obj * 1.0f : (float)variables[1].obj;
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
            if (variables[1].identity.Equals("真值"))
            {
                curGraph.UpdateGlobalVarible(variables[1].name, Mathf.RoundToInt(res));

            }
            else
            {
                curGraph.UpdateGlobalVarible(variables[1].name, res);
            }
        }


        protected internal override void NodeGUI()
        {

            GUILayout.Label("结果：");
            DrawGlobalLayout(variables[0]);
            DrawFillsLayout(variables[1]);
            cType = (CalcuType)EditorGUILayout.EnumPopup(cType);
            DrawFillsLayout(variables[2]);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeCalculation>();
            node.Title = "算术计算";
            node.rect = new Rect(pos, new Vector2(100, 220));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();

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
            base.Start();
            variables[0].setRangeType(this, "真值", "实值");
            variables[1].setRangeType(this, "真值", "实值");
            variables[2].setRangeType(this, "真值", "实值");
        }
    }
}
