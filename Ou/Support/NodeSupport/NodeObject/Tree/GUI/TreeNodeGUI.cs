using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"UGUI","NodeType")]
    public class TreeNodeGUI:TreeNode
    {
        private const string nodeId = "UGUI";
        protected Vector2 scrollVector2=Vector2.zero;
        protected GUISkin gUISkin;
        protected internal override void Evaluator()
        {
            if(gUISkin==null)
                Start();
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {
            base.NodeGUI();
        }

        public override Node Create(Vector2 pos)
        {
            return base.Create(pos);
        }

        protected internal override TreeNodeResult OnUpdate()
        {
            return base.OnUpdate();
        }

        protected internal override void OnStart()
        {
            base.OnStart();
        }

        protected internal override void Start()
        {
            base.Start();
            gUISkin = AssetDatabase.LoadAssetAtPath<GUISkin>(@"Assets/Ou/GUI Skin/Editor/TreeNodeGUI.guiskin");
        }

        public override string GetId { get { return nodeId; } }
    }
}
