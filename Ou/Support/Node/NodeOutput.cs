using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.OuUtility;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Node
{
    public class NodeOutput:NodeKnob
    {
        public string outputType;
        public List<NodeInput> connections = new List<NodeInput>();
        public static NodeOutput Create(Node nodeBody, string outputName, string outputType, Side sd = Side.Left, float offset = 0)
        {
            NodeOutput output = CreateInstance<NodeOutput>();
            output.outputType = outputType;
            offset = offset + 20;
            output.Init(nodeBody, outputName, sd, offset);
            output.texture2D = OuUIUtility.ColorToTex(1, Color.black);
            nodeBody.outputKnobs.Add(output);
            return output;
        }

        public void DrawConnections()
        {
            foreach (NodeInput connection in connections)
            {
                OuUIUtility.DrawLine(this.rect.center, connection.rect.center);
            }
        }
    }
}
