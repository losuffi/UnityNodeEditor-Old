using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.UnitSupport
{
    public static class UnitToolBar
    {

        public static void DrawToolBar(Rect rect, GUISkin skin)
        {
            GUILayout.BeginHorizontal();
            OuUIUtility.FormatButton("保存", UnitEditor.Save, skin.GetStyle("ToolBarButton"));
            OuUIUtility.FormatButton("加载", NodeEditor.LoadCanvas, skin.GetStyle("ToolBarButton"));
            OuUIUtility.FormatButton("新建", UnitEditor.CreateNewUnit, skin.GetStyle("ToolBarButton"));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
    }
}
