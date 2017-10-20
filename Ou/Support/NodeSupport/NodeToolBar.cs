using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
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
