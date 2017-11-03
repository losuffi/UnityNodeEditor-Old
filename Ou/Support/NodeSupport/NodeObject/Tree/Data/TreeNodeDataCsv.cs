using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"CSV数据","Node")]
    public class TreeNodeDataCsv:TreeNodeData
    {
        private string[] firstRows;
        private List<string[]> lexiconCSV=new List<string[]>();
        private bool isInitiazation = false;
        private int selectedDataIndex = 0;
        private int selectedDataRowIndex = 0;
        protected internal override void Evaluator()
        {
            base.Evaluator();
            if (curGraph.CheckKey(variables[1].name))
            {
                curGraph.UpdateGlobalVarible(variables[1].name, variables[2].obj);
            }
        }

        protected internal override void Start()
        {
            base.Start();
            variables[0].setRangeType(this, "CSV");
            variables[1].setRangeType(this, "字符串", "真值", "实值");
            variables[2].setRangeType(this, "字符串", "真值", "实值");
        }

        protected internal override void NodeGUI()
        {
            GUILayout.Label("设置值");
            DrawFillsLayout(variables[0]);
            if (variables[0].obj!=null&&variables[0].obj.GetType() == typeof(TextAsset))
            {
                DrawCSVHandle();
            }
         //   Debug.Log(Selection.activeObject.GetType());
        }

        void DrawCSVHandle()
        {
            if (!isInitiazation)
            {
                CSVHandle();
                return;
            }
            GUILayout.Label("选择行（首元素）：");
            OuUIUtility.FormatPopup(ref selectedDataRowIndex, firstRows);
            GUILayout.Label("选择元素：");
            OuUIUtility.FormatPopup(ref selectedDataIndex,lexiconCSV[selectedDataRowIndex]);
            GUILayout.Label("转化：");
            variables[1].obj = lexiconCSV[selectedDataRowIndex][selectedDataIndex];
            DrawLocalLayout(variables[1]);
            GUILayout.Label("承接变量:");
            DrawGlobalLayout(variables[2]);
        }

        void CSVHandle()
        {
            var CSV = variables[0].obj as TextAsset;
            string[] CSVLinesData = Regex.Split(CSV.text, @"\n");
            firstRows = new string[CSVLinesData.Length];
            for (int i = 0; i < CSVLinesData.Length; i++)
            {
                string[] lexiconRow = CSVLinesData[i].Split(',');
                firstRows[i] = lexiconRow[0];
                lexiconCSV.Add(lexiconRow);
            }
            isInitiazation = true;
        }

        public override Node Create(Vector2 pos)
        {
            TreeNode node = CreateInstance<TreeNodeDataCsv>();
            node.Title = "CSV数据";
            node.rect = new Rect(pos, new Vector2(150, 320));
            node.CreateNodeInput("PreIn", "工作状态");
            node.CreateNodeOutput("Nextout", "工作状态");
            node.CreateVariable();
            node.CreateVariable();
            node.CreateVariable();
            return node;
        }
        private const string nodeId = "CSV数据";
        protected internal override TreeNodeResult OnUpdate()
        {
            return TreeNodeResult.Done;
        }

        protected internal override void OnStart()
        {
        }

        public override string GetId { get { return nodeId; } }
    }
}
