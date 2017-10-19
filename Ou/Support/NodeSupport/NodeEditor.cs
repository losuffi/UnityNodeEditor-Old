using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.OuUtility;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Node
{
    public static class NodeEditor
    {
        private static Texture2D texture2D;
        public static NodeGraph curNodeGraph;
        public static NodeEditorState curNodeEditorState;
        public static NodeInputInfo CurNodeInputInfo;
        private const string Path = @"Assets/Ou/Property/Canvas.asset";

        public static void Clear()
        {
            curNodeGraph.Clear();
        }

        #region GUIDraw
        public static void DrawCanvas(NodeGraph nodeGraph,NodeEditorState nodeEditorState)
        {
            DrawBackground();
            curNodeGraph = nodeGraph;
            curNodeEditorState = nodeEditorState;
            for (int nodeCnt = 0; nodeCnt < curNodeGraph.nodes.Count; nodeCnt++)
            {
                curNodeGraph.nodes[nodeCnt].Draw();
            }

            if (CurNodeInputInfo == null || CurNodeInputInfo.EdState == null) 
            {
                CurNodeInputInfo = new NodeInputInfo("test", curNodeEditorState);
            }
            DrawLink();
            NodeInputSystem.DynamicInvoke(CurNodeInputInfo);
        }

        public static void DrawBackground()
        {
            if (texture2D == null)
            {
                texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Ou/OuSource/background.png");
                return;
            }
            float offsetX = curNodeEditorState.PanOffset.x % texture2D.width;
            float offsetY = curNodeEditorState.PanOffset.y % texture2D.height;
            Vector2 offset = new Vector2(-offsetX / texture2D.width, offsetY / texture2D.height);
            Vector2 Scale =
                new Vector2((curNodeEditorState.CurGraphRect.width / texture2D.width),
                    curNodeEditorState.CurGraphRect.height / texture2D.height);
            Rect uiRect = new Rect(
                offset,Scale/curNodeEditorState.GraphZoom
                );
            Rect rect = curNodeEditorState.CurGraphRect;
            rect.position=Vector2.zero;
            GUI.DrawTextureWithTexCoords(rect, texture2D, uiRect);
        }

        public static void RectConverting(ref Rect rect)
        {
            rect = new Rect(rect.position * curNodeEditorState.GraphZoom, rect.size * curNodeEditorState.GraphZoom);
            rect.position += curNodeEditorState.PanOffset + curNodeEditorState.PanAdjust;
        }

        public static void DrawLink()
        {
            if (curNodeEditorState.IsLinkSetting)
            {
                Vector3 startPos = new Vector3(curNodeEditorState.SelectedKnob.rect.center.x,
                    curNodeEditorState.SelectedKnob.rect.center.y, 0);
                Vector3 endPos = new Vector3(CurNodeInputInfo.InputPos.x, CurNodeInputInfo.InputPos.y, 0);
                OuUIUtility.DrawLine(startPos, endPos);
            }
        }

        public static GenericMenu GetGenericMenu()
        {
            GenericMenu menu=new GenericMenu();
            foreach (var node in NodeTypes.nodes)
            {
                if (NodeAdjust.CurrentNodeType.Equals(string.Empty) ||
                    NodeAdjust.CurrentEditorType.Equals(string.Empty))
                {
                    break;
                }
                if (node.Value.Adress.Contains(NodeAdjust.CurrentEditorType+"|"+NodeAdjust.CurrentNodeType))
                {
                    menu.AddItem(new GUIContent(node.Key.GetId), false, CallBack, node.Key); //需要修改 装入InputControls中。
                }
            }
            return menu;
        }

        static void CallBack(object obj)
        {
            Node node = obj as Node;
            if (curNodeGraph != null)
            {
                Vector2 pos = CurNodeInputInfo.InputPos - curNodeEditorState.CurGraphRect.position;
                curNodeGraph.AddNode(node, pos);
            }
        }
        #endregion
        #region DataSave

        public static void SaveCurrentCanvas()
        {
            EditorUtility.SetDirty(curNodeEditorState);
            AssetDatabase.SaveAssets();

        }

        public static void InitAssetData(out NodeEditorState state,out NodeGraph graph)
        {
            state = AssetDatabase.LoadAssetAtPath<NodeEditorState>(Path);
            if (state!=null)
            {
                graph = state.CurGraph;
            }
            else
            {
                state = ScriptableObject.CreateInstance<NodeEditorState>();
                graph = ScriptableObject.CreateInstance<NodeGraph>();
                state.CurGraph = graph;
                AssetDatabase.CreateAsset(state, Path);
                AssetDatabase.AddObjectToAsset(graph, state);
            }
            CurNodeInputInfo = null;
        }
        #endregion
    }
}
