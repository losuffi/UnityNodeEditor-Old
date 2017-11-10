using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public class runTimeMac : MonoBehaviour
    {
        [SerializeField] protected NodeGraph graph;

        public void set(NodeGraph graph)
        {
            this.graph = graph;
        }

    }
}
