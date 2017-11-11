using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.UnitSupport;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false, "Unit-文本框设置B", "Node")]
    public class TreeNodeUGUIUnitText_B : TreeNodeGUI
    {
        [SerializeField] private int Count;
        public override string GetId { get { return "Unit-文本框设置B"; } }
        // [SerializeField] private string content = string.Empty;
        protected internal override void Evaluator()
        {
            UnitBase unit = curGraph.ReadGlobalVariable(variables[0]).obj as UnitBase;
            for (int i = 0; i < Count; i++)
            {
                var text = variables[1 + 2 * i].obj as Text;
                GlobalVariable res = unit.ReadGlobalVariable(variables[2+2*i].name);
                text.text = res.obj.ToString();
            }
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            OuUIUtility.FormatLabel("Unit:");
            DrawFillsLayout(variables[0]);
            OuUIUtility.FormatLabel("Count:");
            OuUIUtility.FormatIntfield(ref Count);
            if (variables[0].obj != null && variables[0].obj.GetType() == typeof(UnitBase))
            {
                UnitBase tar = variables[0].obj as UnitBase;
                if ((Count * 2) + 1 != variables.Count)
                {
                    UpdateData(tar);
                }
                scrollVector2 = GUILayout.BeginScrollView(scrollVector2);
                DrawUnitHandle();
                GUILayout.EndScrollView();
            }
        }

        private void UpdateData(UnitBase tar)
        {
            var deviation = Count * 2 + 1 - variables.Count;
            Debug.Log(deviation);
            if (deviation<0)
            {
                variables.RemoveRange((Count * 2) + 1, -deviation);
            }
            else
            {
                for (int i = 0; i < deviation; i+=2)
                {
                    var ui = CreateVariable();
                    ui.setRangeType(this, "TextUI");
                    var v= CreateVariable();
                    v.setRangeType(tar);
                }
            }
            rect = new Rect(rect.position, new Vector2(180, 350));
        }

        private void DrawUnitHandle()
        {
            for (int i = 0; i < Count; i++)
            {
                OuUIUtility.FormatLabel("UI目标");
                DrawFillsLayout(variables[1 + (2 * i)]);
                OuUIUtility.FormatLabel("读取属性：");
                DrawUnitLayout(variables[2 + (2 * i)]);
            } 
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUIUnitText_B>();
            node.Title = "Unit-文本框设置B";
            node.rect = new Rect(pos, new Vector2(150, 240));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
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
            variables[0].setRangeType(this, "Unit");
            base.Start();
        }
    }
}
