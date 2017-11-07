using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false, "UI连线", "Node")]
    public class TreeNodeUGUILine : TreeNodeGUI
    {
        public override string GetId { get { return "UI连线"; } }
        [SerializeField]
        private Color colFill=Color.black;

        [SerializeField] private bool active = false;
        // [SerializeField] private string content = string.Empty;
        protected internal override void Evaluator()
        {
            var manager = TreeNodeManager.Instance.gameObject.GetComponent<DrawLine>();
            if (manager == null)
            {
                manager = TreeNodeManager.Instance.gameObject.AddComponent<DrawLine>();
            }
            GameObject v1=variables[0].obj as GameObject;
            GameObject v2 = variables[1].obj as GameObject;
            if (active)
            {
                manager.lines.Add(new LinePoints(v1.transform.localPosition, v2.transform.localPosition,
                    v1.transform.name + "UILines" + v2.transform.name,
                    colFill));
            }
            else
            {
                manager.lines.RemoveAll(res => res.lineName.Equals(v1.transform.name + "UILines" + v2.transform.name));
            }
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            OuUIUtility.FormatLabel("连线颜色");
            colFill = EditorGUILayout.ColorField(colFill);
            OuUIUtility.FormatLabel("连线起始目标");
            DrawFillsLayout(variables[0]);
            OuUIUtility.FormatLabel("连线终止目标");
            DrawFillsLayout(variables[1]);
            active = GUILayout.Toggle(active, "显示/隐藏");
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUILine>();
            node.Title = "UI连线";
            node.rect = new Rect(pos, new Vector2(150, 240));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
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
            //  ext = new GlobalVariable(typeof(Text), null, "TextUI", "text");
            variables[0].setRangeType(this, "ObjectUI");
            variables[1].setRangeType(this, "ObjectUI");
            base.Start();
        }
    }
}
