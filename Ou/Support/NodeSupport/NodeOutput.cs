using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public class NodeOutput:NodeKnob
    {
        public string outputType;
        public List<NodeInput> connections = new List<NodeInput>();
        public ConnectionTypeData ctd;
        public object Value = null;
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
        protected override void CheckColor()
        {
            base.CheckColor();
            texture2D = OuUIUtility.ColorToTex(1, Color.black);
        }
        public void DrawConnections()
        {
            foreach (NodeInput connection in connections)
            {
                OuUIUtility.DrawLine(this.rect.center, connection.rect.center);
            }
        }

        void Check()
        {
            if(ctd!=null)
                return;
            if (ConnectionType.types.ContainsKey(outputType))
            {
                this.ctd = ConnectionType.types[outputType];
            }
            else
            {
                this.ctd = null;
                throw new UnityException("节点：" + Body.GetId + "-" + Body.Title + "的输出端口设置的类型，是未定义的类型！");
            }
        }
        public void SetValue<T>(T obj)
        {
            Check();
            if (!ctd.type.IsAssignableFrom(typeof(T)))
            {
                throw new UnityException(Body.GetId + "-" + Body.Title + "输出端口数据类型不匹配");
            }
            Value = obj;
        }

        public static T GetDefault<T>()
        {
            if (typeof(T).GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance<T>();
            return default(T);
        }

        public T GetValue<T>()
        {
            Check();
            if (typeof(T).IsAssignableFrom(ctd.type))
                return (T)(Value ?? (Value = GetDefault<T>()));
            return GetDefault<T>();
        }

        public object GetValue()
        {
            return Value;
        }
    }
}
