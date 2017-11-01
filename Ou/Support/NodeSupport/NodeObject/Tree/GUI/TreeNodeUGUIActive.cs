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
        [SerializeField] private GlobalVariable ext;
        protected internal override void Evaluator()
        {
            base.Evaluator();
            var gobj = ext.obj as GameObject;
            if(gobj==null)
                return;
            gobj.SetActive(flag);
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("UI目标");
            ConnectionType.types["ObjectUI"].GUILayout(ref ext.obj);
            GUILayout.Label("是否显示");
            flag = EditorGUILayout.Toggle(flag);
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeUGUIActive>();
            node.Title = "点击等待";
            node.rect = new Rect(pos, new Vector2(60, 60));
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
            ext = new GlobalVariable(typeof(GameObject), null, "ObjectUI", "obj");
            base.Start();
        }
    }
}
