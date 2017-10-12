using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ou.Support.Node
{
    public static class NodeEditor
    {
        public static NodeGraph curNodeGraph;
        public static NodeEditorState curNodeEditorState;
        public static void DrawCanvas(NodeGraph nodeGraph,NodeEditorState nodeEditorState)
        {
            curNodeGraph = nodeGraph;
            curNodeEditorState = nodeEditorState;
            for (int nodeCnt = 0; nodeCnt < curNodeGraph.nodes.Count; nodeCnt++)
            {
                curNodeGraph.nodes[nodeCnt].Draw();
            }
        }

        public static void Clear()
        {
            curNodeGraph.Clear();
        }
    }
}
