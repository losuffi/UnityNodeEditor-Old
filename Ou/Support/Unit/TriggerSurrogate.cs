using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.Node;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Unit
{
    public class TriggerSurrogate:MonoBehaviour
    {
        public NodeEditorState state;
        public NodeGraph graph;
        //Work :需要搭建， Test Vision；
        void Start()
        {
            NodeEditor.InitAssetData(out state, out graph);
            graph.Run();
        }

    }
}
