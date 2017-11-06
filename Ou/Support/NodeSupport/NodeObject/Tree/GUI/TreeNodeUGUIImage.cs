using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false, "贴图设置", "Node")]
    public class TreeNodeUGUIImage : TreeNodeGUI
    {
        public override string GetId { get { return "贴图设置"; } }
        [SerializeField] private Sprite content = null;
        protected internal override void Evaluator()
        {
            var img = variables[0].obj as Image;
            img.sprite = content;
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("贴图设置");
            GUILayout.Label("UI目标");
            DrawFillsLayout(variables[0]);
            GUILayout.Label("贴图:");
            content=EditorGUILayout.ObjectField(content, typeof(Sprite), true) as Sprite;
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUIImage>();
            node.Title = "贴图设置";
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
            variables[0].setRangeType(this, "ImageUI");
            base.Start();
        }
    }
}
