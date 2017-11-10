using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public class NodeManager:MonoBehaviour
    {
        protected NodeEditorState currentState;
        protected NodeGraph currentGraph;
        [SerializeField]
        protected List<NodeGraph> graphs=new List<NodeGraph>();
        protected List<runTimeMac> macs=new List<runTimeMac>();

        protected internal virtual void RegisterGraph(NodeGraph graph)
        {
        }

        public int graphsCount
        {
            get { return graphs.Count; }
        }
    }
}
