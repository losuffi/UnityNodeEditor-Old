using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.Node;
using Ou.Support.OuUtility;
using UnityEngine;

namespace Assets.Ou.Support.Node
{
    public static class NodeToolBar
    {
        public static void DrawToolBar(Rect rect)
        {
            OuUIUtility.FormatButton("Save", NodeEditor.SaveCurrentCanvas, new Vector2(rect.height, rect.height));
            GUILayout.Space(10);
        }
    }
}
