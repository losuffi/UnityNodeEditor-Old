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

        public Node CheckFocus(Vector2 pos)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].nodeRect.Contains(pos))
                {
                    return nodes[i];
                }
            }
            return null;
        }
    }
}
