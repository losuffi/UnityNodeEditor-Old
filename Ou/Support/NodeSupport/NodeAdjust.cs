using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ou.Support.OuUtility;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Node
{
    public static class NodeAdjust
    {
        static Dictionary<string,bool> EditorTypes=new Dictionary<string, bool>();
        static Dictionary<string, bool>  nodesTypes = new Dictionary<string, bool>();
        public static string CurrentEditorType=string.Empty;
        public static string CurrentNodeType=string.Empty;

        private static string currentEditorTypeCache = string.Empty;
        private static string currentNodeTypeCache = string.Empty;
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
            GUILayout.Label("---NodeType----", skin.GetStyle("adjustBodyLabel"));
            DrawNodeTypeToggles(skin);
            #endregion
        }

        static void DrawEditorTypeToggles(GUISkin skin)
        {
            if (!EditorTypes.Any())
            {
                foreach (var node in NodeTypes.nodes)
                {
                    var str = node.Value.Adress.Split('|')[0];
                    if (!EditorTypes.Keys.Contains(str))
                    {
                        EditorTypes.Add(str, false);
                    }
                }
            }
            foreach (var key in new List<string>(EditorTypes.Keys))
            {
                if (GUILayout.Toggle(EditorTypes[key], new GUIContent(key), skin.toggle))
                {
                    CurrentEditorType = key;
                    if (!CurrentEditorType.Equals(currentEditorTypeCache))
                        SelectedEditorType(CurrentEditorType);
                }
            }

        }

        static void DrawNodeTypeToggles(GUISkin skin)
        {
            if (CurrentEditorType.Equals(string.Empty))
            {
                return;
            }
            if (!nodesTypes.Any())
            {
                foreach (var node in NodeTypes.nodes)
                {
                    if (node.Value.Adress.Contains(CurrentEditorType))
                    {
                        var str = node.Value.Adress.Split('|')[1];
                        if (!nodesTypes.Keys.Contains(str))
                        {
                            nodesTypes.Add(str, false);
                        }
                    }
                }
            }
            foreach (var key in new List<string>(nodesTypes.Keys))
            {
                if (GUILayout.Toggle(nodesTypes[key], new GUIContent(key), skin.toggle))
                {
                    CurrentNodeType = key;
                    if(!CurrentNodeType.Equals(currentNodeTypeCache))
                        SelectedNodeType(key);
                }
            }
        }

        static void SelectedEditorType(string type)
        {
            if (type.Equals(string.Empty))
                return;
            var keys = new List<string>(EditorTypes.Keys);
            foreach (var key in keys)
            {
                EditorTypes[key] = false;
            }
            EditorTypes[type] = true;
            currentEditorTypeCache = type;
            CurrentNodeType = string.Empty;
            currentNodeTypeCache = string.Empty;
            nodesTypes.Clear();
        }

        static void SelectedNodeType(string type)
        {
            if (type.Equals(string.Empty))
                return;
            foreach (var key in new List<string>(nodesTypes.Keys))
            {
                nodesTypes[key] = false;
            }
            nodesTypes[type] = true;
            currentNodeTypeCache = type;
        }
    }
}
