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

        private static Dictionary<string,SelectedTypeData> editorTypeDatas=new Dictionary<string, SelectedTypeData>();
        public static string selectedEditorTypeName = string.Empty;
        public static Dictionary<string, SelectedTypeData> nodeTypeDatas = new Dictionary<string, SelectedTypeData>();
        public static string selectedNodeTypeName = string.Empty;
        public static void Draw(GUISkin skin)
        {
            #region Handle
            GUILayout.Label("---操作---", skin.GetStyle("adjustBodyLabel"));
            OuUIUtility.FormatButton("Clear", NodeEditor.Clear, skin.GetStyle("adjustBodyButton"));
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
    }
}
