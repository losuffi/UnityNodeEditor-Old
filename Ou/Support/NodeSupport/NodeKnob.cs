﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Serializable]
    public enum Side
    {
        Left,
        Top,
        Right,
        Bottom,
    }
    public class NodeKnob:ScriptableObject
    {
        [SerializeField]
        protected internal Node Body;
        [SerializeField]
        protected internal string Name;
        [SerializeField]
        protected Side side;
        [SerializeField]
        protected float sideOffset;
        [SerializeField]
        protected Texture2D texture2D;
        public Rect rect;
        protected void Init(Node bodyNode, string knobName, Side sd,float offset)
        {
            Body = bodyNode;
            Name = knobName;
            side = sd;
            sideOffset = offset;
            bodyNode.Knobs.Add(this);
        }
        protected virtual void CheckColor()
        {
            
        }
        public virtual void Draw()
        {
            rect = GetGUIKnob();
            NodeEditor.RectConverting(ref rect);
            if(texture2D==null)
                CheckColor();
            GUI.DrawTexture(rect, texture2D);
        }

        private Rect GetGUIKnob()
        {
            Vector2 pos = Body.rect.position;
            Vector2 size=new Vector2(10,10);
            if (side == Side.Left)
            {
                pos = new Vector2(pos.x - size.x, pos.y + sideOffset);
            }
            else if(side== Side.Top)
            {
                pos = new Vector2(pos.x + sideOffset, pos.y - size.y);
            }
            else if (side == Side.Right)
            {
                pos = new Vector2(pos.x+Body.rect.width, pos.y + sideOffset);
            }
            else if (side == Side.Bottom)
            {
                pos = new Vector2(pos.x + sideOffset, pos.y + +Body.rect.height);
            }
            return new Rect(pos, size);
        }

        public static void Linking(NodeOutput output, NodeInput input)
        {
            output.connections.Add(input);
            input.connection = output;
        }
    }
}
