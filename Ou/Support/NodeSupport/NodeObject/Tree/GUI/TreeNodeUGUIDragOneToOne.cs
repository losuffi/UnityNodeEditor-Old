using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false, "UI拖拽1To1", "Node")]
    public class TreeNodeUGUIDragOneToOne : TreeNodeGUI
    {
        public override string GetId { get { return "UI拖拽1To1"; } }
        [SerializeField] private Rect rectTrigger;
        [SerializeField] private Rect rectTarget;
        private RectTransform triTransform;
        private RectTransform tarTransform;
        private Vector3 mousePos;
        private Vector3 triCachePos;
        private bool isDragging;
        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            OuUIUtility.FormatLabel("UI工作目标");
            DrawFillsLayout(variables[0]);
            OuUIUtility.FormatLabel("UI位置目标");
            DrawFillsLayout(variables[1]);
            OuUIUtility.FormatLabel("贴图:");
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUIDragOneToOne>();
            node.Title = "UI拖拽1To1";
            node.rect = new Rect(pos, new Vector2(120, 200));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            node.CreateVariable();
            return node;

        }

        protected internal override TreeNodeResult OnUpdate()
        {
            mousePos = Input.mousePosition;
            if (!isDragging)
            {
                if (rectTrigger.Contains(mousePos) && Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                }
            }
            else
            {
                triTransform.position = mousePos;
                if (Input.GetMouseButtonUp(0))
                {
                    triTransform.position = triCachePos;
                    isDragging = false;
                    if (rectTarget.Contains(mousePos))
                    {
                        return TreeNodeResult.Done;
                    }
                }
            }
            return TreeNodeResult.Running;
        }

        protected internal override void OnStart()
        {
            isDragging = false;
            var trigger = variables[0].obj as GameObject;
            var target = variables[1].obj as GameObject;
            triTransform = trigger.GetComponent<RectTransform>();
            tarTransform = target.GetComponent<RectTransform>();
            if (tarTransform == null || triTransform == null)
            {
                throw new InvalidOperationException("输入参数不当，请确认为UGUI元素");
            }
            triCachePos = triTransform.position;
            rectTrigger = new Rect(new Vector2(triTransform.position.x - triTransform.sizeDelta.x / 2, triTransform.position.y - triTransform.sizeDelta.y / 2), triTransform.sizeDelta);
            rectTarget = new Rect(new Vector2(tarTransform.position.x - tarTransform.sizeDelta.x / 2, tarTransform.position.y - tarTransform.sizeDelta.y / 2), tarTransform.sizeDelta);
            base.OnStart();
        }

        protected internal override void Start()
        {
            variables[0].setRangeType(this, "ObjectUI");
            variables[1].setRangeType(this, "ObjectUI");
            base.Start();
        }
    }
}
