using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ou.Support.Node
{
    public  class NodeGraph: ScriptableObject
    {
        //TODO:NodeCanvas Configue
        public List<Node> nodes=new List<Node>();

        public void Clear()
        {
            if(nodes.Any())
                nodes.Clear();
        }
    }
}
