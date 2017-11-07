using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false, "UI显示隐藏", "Node")]
    public class TreeNodeUGUIActive : TreeNodeGUI
    {
        public override string GetId { get { return "UI显示隐藏"; } }
        [SerializeField] private bool flag;
        protected internal override void Evaluator()
        {
            base.Evaluator();
            GameObject gobj = variables[0].obj as GameObject;
            if(gobj==null)
                return;
            gobj.SetActive(flag);
        }

        protected internal override void NodeGUI()
        {
            OuUIUtility.FormatLabel("UI目标");
            DrawFillsLayout(variables[0]);
            OuUIUtility.FormatLabel("是否显示");
            flag = EditorGUILayout.Toggle(flag);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUIActive>();
            node.Title = "UI显示隐藏";
            node.rect = new Rect(pos, new Vector2(120, 120));
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
            //ext = new GlobalVariable(typeof(GameObject), null, "ObjectUI", "obj");
            variables[0].setRangeType(this, "ObjectUI");
            base.Start();
        }
    }
}
