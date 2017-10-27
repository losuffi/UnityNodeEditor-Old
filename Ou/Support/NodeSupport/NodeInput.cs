using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public class NodeInput:NodeKnob
    {
        public string InputType;
        public NodeOutput connection;
        public static NodeInput Create(Node nodeBody, string inputName, string inputType,Side sd= Side.Top,float offset = 0) 
        {
            NodeInput input = CreateInstance<NodeInput>();
            input.InputType = inputType;
            offset += 20;
            input.Init(nodeBody, inputName, sd, offset);
            SetKnobUI(input);
            nodeBody.inputKnobs.Add(input);
            input.Body = nodeBody;
            return input;
        }


        static void SetKnobUI(NodeInput input)
        {
            if (input.Name.Contains("PreIn"))
            {
                input.texture2D = OuUIUtility.ColorToTex(1, Color.red);
            }
        }

        protected override void CheckColor()
        {
            base.CheckColor();
            SetKnobUI(this);
        }

        public void DrawConnections()
        {
            if(connection!=null)
                OuUIUtility.DrawLine(connection.rect.center, this.rect.center);
        }

        public T GetValue<T>()
        {
            return connection != null ? connection.GetValue<T>() : NodeOutput.GetDefault<T>();
        }

        public void SetValue<T>(T obj)
        {
            if (connection != null)
            {
                connection.SetValue<T>(obj);
            }
        }
    }
}
