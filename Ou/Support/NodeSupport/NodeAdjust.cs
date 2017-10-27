using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public static class NodeAdjust
    {
        public class SelectedTypeData
        {
            public Type type;
            public bool isSelected;

            public SelectedTypeData(Type type)
            {
                this.type = type;
                isSelected = false;
            }
        }

        private static string drawIdentity = "Normal";

        private static GUISkin skin = null;
        private static Dictionary<string,SelectedTypeData> editorTypeDatas=new Dictionary<string, SelectedTypeData>();
        public static string selectedEditorTypeName = string.Empty;
        public static Dictionary<string, SelectedTypeData> nodeTypeDatas = new Dictionary<string, SelectedTypeData>();
        public static string selectedNodeTypeName = string.Empty;

        private static int FillIndex = 0;
        private static GlobalVariable obj = new GlobalVariable(typeof(string), string.Empty, "字符串", string.Empty);

        public static void Draw(GUISkin skin)
        {
            NodeAdjust.skin = skin;
            if (drawIdentity.Equals("Normal"))
            {
                DrawTreeNodeNormal();
            }else if (drawIdentity.Equals("Variable"))
            {
                DrawTreeNodeVariable();
            }
        }

        #region NodeType

        private static void DrawNodeTypeToggles(GUISkin skin)
        {
            if (!nodeTypeDatas.Any())
            {
                NodeTypesInitialzaion();
            }
            else
            {
                foreach (string key in new List<string>(nodeTypeDatas.Keys))
                {
                    if (TriggerEditorUtility.LayoutCheck())
                    {
                        if (GUILayout.Toggle(nodeTypeDatas[key].isSelected, new GUIContent(key), skin.toggle))
                        {
                            SelectedNodeType(key);
                        }
                    }
                }
            }
        }

        private static void SelectedNodeType(string key)
        {
            if (selectedNodeTypeName.Equals(key))
            {
                return;
            }
            selectedNodeTypeName = key;
            foreach (string s in new List<string>(nodeTypeDatas.Keys))
            {
                nodeTypeDatas[s].isSelected = false;
            }
            nodeTypeDatas[selectedNodeTypeName].isSelected = true;
        }

        private static void NodeTypesInitialzaion()
        {
            if (selectedEditorTypeName.Equals(string.Empty))
            {
                return;
            }
            foreach (var nodeData in NodeTypes.nodes)
            {
                if (nodeData.Value.Identity.Equals("NodeType")&&nodeData.Value.type.IsSubclassOf(editorTypeDatas[selectedEditorTypeName].type))
                {
                    nodeTypeDatas.Add(nodeData.Value.Name, new SelectedTypeData(nodeData.Value.type));
                }
            }
        }

        private static void DrawEditorTypeToggles(GUISkin skin)
        {
            if (!editorTypeDatas.Any())
            {
                EditorTypesInitialzation();
            }
            else
            {
                foreach (var key in new List<string>(editorTypeDatas.Keys))
                {
                    if (TriggerEditorUtility.LayoutCheck())
                    {
                        if (GUILayout.Toggle(editorTypeDatas[key].isSelected, new GUIContent(key), skin.toggle))
                        {
                            SelectedEditorType(key);
                        }
                    }
                }
            }
        }

        private static void EditorTypesInitialzation()
        {
            foreach (var nodeData in NodeTypes.nodes)
            {
                if (nodeData.Value.Identity.Equals("EditorType"))
                {
                    editorTypeDatas.Add(nodeData.Value.Name, new SelectedTypeData(nodeData.Value.type));
                }
            }
        }

        private static void SelectedEditorType(string name)
        {
            if (selectedEditorTypeName.Equals(name))
            {
                return;
            }
            selectedEditorTypeName = name;
            foreach (string key in new List<string>(editorTypeDatas.Keys))
            {
                editorTypeDatas[key].isSelected = false;
            }
            editorTypeDatas[selectedEditorTypeName].isSelected = true;
            selectedNodeTypeName = string.Empty;
            TriggerEditorUtility.IsLayout = false;
            nodeTypeDatas.Clear();
        }
        #endregion

        #region DrawType

        private static void DrawTreeNodeNormal()
        {
            #region Handle
            GUILayout.Label("---操作---", skin.GetStyle("adjustBodyLabel"));
            OuUIUtility.FormatButton("Register", NodeEditor.RegisterTreeManager, skin.GetStyle("adjustBodyButton"));
            OuUIUtility.FormatButton("Clear", NodeEditor.Clear, skin.GetStyle("adjustBodyButton"));
            NodeEditor.curNodeEditorState.Name = GUILayout.TextArea(NodeEditor.curNodeEditorState.Name, skin.GetStyle("adjustBodyTextArea"));
            OuUIUtility.FormatButton("SetName", NodeEditor.RemDataAsset, skin.GetStyle("adjustBodyButton"));
            OuUIUtility.FormatButton("Global Variable", () => { drawIdentity = "Variable";
                Selection.activeObject = NodeEditor.curNodeGraph;
            }, skin.GetStyle("adjustBodyButton"));
            #endregion

            #region EditorType
            GUILayout.Label("---工作模式----", skin.GetStyle("adjustBodyLabel"));
            GUILayout.BeginHorizontal();
            DrawEditorTypeToggles(skin);
            GUILayout.EndHorizontal();
            #endregion

            #region NodeType
            GUILayout.Label("---节点种类----", skin.GetStyle("adjustBodyLabel"));
            DrawNodeTypeToggles(skin);
            #endregion
        }

        private static void DrawTreeNodeVariable()
        {
            GUILayout.Label("---全局变量库---", skin.GetStyle("adjustBodyLabel"));
            for (int i=0;i<NodeEditor.curNodeGraph.globalVariables.Count;i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(NodeEditor.curNodeGraph.globalVariables[i].name);
                //GUILayout.Label(NodeEditor.curNodeGraph.globalVariables[i].Value.obj.ToString());
                OuUIUtility.FormatButton("-", () => { NodeEditor.curNodeGraph.globalVariables.RemoveAt(i);
                    i--;
                });
                GUILayout.EndHorizontal();
            }
            GUILayout.Label("---添加变量---", skin.GetStyle("adjustBodyLabel"));
            GUILayout.Label("变量名：");
            OuUIUtility.FormatTextfield(ref obj.name);
            GUILayout.Label("变量种类：");
            OuUIUtility.FormatSetVariable_SelectedType(ref obj, ref FillIndex);
            OuUIUtility.FormatButton("添加",AddVariable);


            GUILayout.Space(20);
            OuUIUtility.FormatButton("返回", () => { drawIdentity = "Normal"; }, skin.GetStyle("adjustBodyButton"));
        }

        private static void AddVariable()
        {
            NodeEditor.curNodeGraph.AddGlobalVariable(obj);
            NodeEditor.curNodeGraph.nodes.ForEach(z=>z.Start());
            obj=new GlobalVariable(typeof(string),string.Empty,"字符串",string.Empty);
        }
        #endregion
    }
}
