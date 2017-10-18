using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.OuUtility;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Node
{
    public class NodeInput:NodeKnob
    {
        public string InputType;
        public NodeOutput connection;
        public static NodeInput Create(Node nodeBody, string inputName, string inputType,Side sd= Side.Left,float offset = 0) 
        {
            NodeInput input = CreateInstance<NodeInput>();
            input.InputType = inputType;
            offset += 20;
            input.Init(nodeBody, inputName, sd, offset);
            input.texture2D = OuUIUtility.ColorToTex(1, Color.red);
            nodeBody.inputKnobs.Add(input);
            return input;
        }
        public void DrawConnections()
        {
            if(connection!=null)
                OuUIUtility.DrawLine(connection.rect.center, this.rect.center);
        }
    }
}
