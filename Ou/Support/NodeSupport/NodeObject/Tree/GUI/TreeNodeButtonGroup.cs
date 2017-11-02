﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ou.Support.NodeSupport
{
    [Node(false,"按钮组-A","Node")]
    public class TreeNodeButtonGroup:TreeNodeGUI
    {
        public override string GetId { get { return "按钮组"; } }
        private bool flag;
        private Vector2 scrVector2 = Vector2.zero;
        [SerializeField] private int Count = 0;
        [SerializeField] private GlobalVariable ext;
        [SerializeField] private List<NodeOutput> btnsOutputs = new List<NodeOutput>();
        [SerializeField] private List<string> strNames=new List<string>();
        protected internal override void Evaluator()
        {
            base.Evaluator();
        }

        protected internal override void NodeGUI()
        {

            GUILayout.Label("显示位置");
            GUILayout.Label("UI目标");
            ConnectionType.types["ButtonGroupUI"].GUILayout(ref ext.obj);
            GUILayout.Label("按钮数目:");
            OuUIUtility.FormatIntfield(ref Count);
            if (btnsOutputs.Count != Count)
            {
                UpdateBtnsTriggerPipe();
            }
            GUILayout.Label("按钮名：");
            scrVector2 = GUILayout.BeginScrollView(scrVector2);
            for (int i = 0; i < strNames.Count; i++)
            {
                GUILayout.Label(i.ToString());
                strNames[i] = GUILayout.TextField(strNames[i]);
            }
            GUILayout.EndScrollView();
        }

        void UpdateBtnsTriggerPipe()
        {
            Debug.Log("update");
            foreach (NodeOutput btnsOutput in btnsOutputs)
            {
                DestroyImmediate(btnsOutput,true);
            }
            rect = new Rect(rect.position, new Vector2(120, 200));
            Knobs.RemoveAll(z => z.Name.Equals("Btns"));
            btnsOutputs.Clear();
            strNames.Clear();
            if (Count <= 0)
                return;
            int half = Count / 2;
            for (int i = 0; i < half; i++)
            {
                var output= CreateNodeOutput("Btns", "工作状态", Side.Left, i * 30);
                AssetDatabase.AddObjectToAsset(output,this);
                btnsOutputs.Add(output);
                strNames.Add(string.Empty);
            }
            for (int j = half; j < Count; j++)
            {
                var output = CreateNodeOutput("Btns", "工作状态", Side.Right, (j-half) * 30);
                AssetDatabase.AddObjectToAsset(output, this);
                btnsOutputs.Add(output);
                strNames.Add(string.Empty);
            }
            rect = new Rect(rect.position, new Vector2(120, 200 + (Count / 2) * 30));
            outputKnobs.RemoveAll(z => z.Name.Equals("Btns"));
        }

        public override Node Create(Vector2 pos)
        {
            Node node = CreateInstance<TreeNodeButtonGroup>();
            node.Title = "按钮组";
            node.rect = new Rect(pos, new Vector2(120, 200));
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
            var btns = ext.obj as ButtonGroup;
            for (int i = 0; i < Count; i++)
            {
                var btn = btns.AddButton("btn" + i, strNames[i]);
                btn.onClick.RemoveAllListeners();
                int j = i;
                btn.onClick.AddListener(() => { btnsOutputs[j].SetValue<TreeNodeResult>(TreeNodeResult.Start); });
            }

            base.OnStart();
        }

        protected internal override void Start()
        {
            ext = new GlobalVariable(typeof(ButtonGroup), null, "ButtonGroupUI", "btnsG");
            base.Start();
        }
    }
}