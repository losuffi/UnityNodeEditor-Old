using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false, "UI简易动画", "Node")]
    public class TreeNodeUGUISimpleAnim : TreeNodeGUI
    {
        public override string GetId { get { return "UI简易动画"; } }
        [SerializeField] private bool isLoop;
        private bool isComplete;
        private float initTime;
        private Vector3 cache;
        protected internal override void Evaluator()
        {
            base.Evaluator();
            sizeEndAnim();
        }

        protected internal override void NodeGUI()
        {
            OuUIUtility.FormatLabel("UI目标");
            DrawFillsLayout(variables[0]);
            OuUIUtility.FormatLabel("动画时间");
            DrawFillsLayout(variables[1]);
            OuUIUtility.FormatLabel("起始因数");
            DrawFillsLayout(variables[2]);
            OuUIUtility.FormatLabel("结束因数");
            DrawFillsLayout(variables[3]);
            OuUIUtility.FormatLabel("是否循环");
            isLoop = EditorGUILayout.Toggle(isLoop);
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeUGUISimpleAnim>();
            node.Title = "UI简易动画";
            node.rect = new Rect(pos, new Vector2(120, 200));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateNodeOutput("Forout", "工作状态", Side.Right,30);
            node.CreateVariable();
            node.CreateVariable();
            node.CreateVariable();
            node.CreateVariable();
            return node;

        }

        protected internal override TreeNodeResult OnUpdate()
        {
            if (isLoop)
            {
                CallFeedBack();
                Goto(GotoType.Single, "Forout"); 
                var res = feedback;
                if (res == TreeNodeResult.Break)
                    return TreeNodeResult.Done;
            }
            if (Anim())
            {
                return TreeNodeResult.Done;
            }
            return TreeNodeResult.Running;
        }

        float Curve()
        {
            float minusVal = (Time.time - initTime);
            float percentVal = minusVal / (float) variables[1].obj;
            float startVal = (float)variables[2].obj;
            float endVal = (float) variables[3].obj;
            float resultDynamicVal = (endVal - startVal) * percentVal;
            float resultVal = startVal + resultDynamicVal;
            return resultVal;
        }

        void EndAnim()
        {
            GameObject obj = variables[0].obj as GameObject;
            obj.transform.localScale = cache;
        }
        bool Anim()
        {
            GameObject obj=variables[0].obj as GameObject;
            float TimeLine = (float)variables[1].obj;
            if (obj==null)
                return true;
            if (isComplete)
            {
                cache = obj.transform.localScale;
                isComplete = false;
            }
            obj.transform.localScale = cache * Curve();
            if (isLoop)
            {
                if (Time.time - initTime > TimeLine)
                {
                    obj.transform.localScale = cache;
                    initTime = Time.time;
                }
                return false;
            }
            else
            {
                if (Time.time - initTime > TimeLine)
                {
                    return true;
                }
            }
            return false;
        }
        protected internal override void OnStart()
        {
            base.OnStart();
            initTime = Time.deltaTime;
            isComplete = true;
        }

        protected internal override void Start()
        {
            //ext = new GlobalVariable(typeof(GameObject), null, "ObjectUI", "obj");
            variables[0].setRangeType(this, "ObjectUI");
            variables[1].setRangeType(this, "实值");
            variables[2].setRangeType(this, "实值");
            variables[3].setRangeType(this, "实值");
            base.Start();
        }
    }
}
