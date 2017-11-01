using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false,"UI按钮点击等待","Node")]
    public class TreeNodeUGUIButton:TreeNodeGUI
    {
        public override string GetId { get { return "UI按钮点击等待"; } }
        private bool flag;
        [SerializeField] private GlobalVariable ext;
        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("UI目标");
            ConnectionType.types["ButtonUI"].GUILayout(ref ext.obj);
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeUGUIButton>();
            node.Title = "点击等待";
            node.rect = new Rect(pos, new Vector2(60, 60));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            return node;

        }

        protected internal override TreeNodeResult OnUpdate()
        {
            if (flag)
                return TreeNodeResult.Done;
            return TreeNodeResult.Running;
        }

        protected internal override void OnStart()
        {
            base.OnStart();
            flag = false;
            var btn = ext.obj as Button;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(()=> { flag = true; });
        }

        protected internal override void Start()
        {
            ext = new GlobalVariable(typeof(Button), null, "ButtonUI", "btn");
            base.Start();
        }
    }
}
