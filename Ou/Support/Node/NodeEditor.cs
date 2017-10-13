using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.Node
{
    public static class NodeEditor
    {
        public static NodeGraph curNodeGraph;
        public static NodeEditorState curNodeEditorState;
        public static NodeInputInfo CurNodeInputInfo;
        public static void DrawCanvas(NodeGraph nodeGraph,NodeEditorState nodeEditorState)
        {
            curNodeGraph = nodeGraph;
            curNodeEditorState = nodeEditorState;
            for (int nodeCnt = 0; nodeCnt < curNodeGraph.nodes.Count; nodeCnt++)
            {
                curNodeGraph.nodes[nodeCnt].Draw();
            }

            if (CurNodeInputInfo == null)
            {
                CurNodeInputInfo = new NodeInputInfo("test", curNodeEditorState);
            }
            NodeInputSystem.DynamicInvoke(CurNodeInputInfo);
        }

        public static void Clear()
        {
            curNodeGraph.Clear();
        }
    }
}
