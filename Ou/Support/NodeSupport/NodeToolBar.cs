using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public static class NodeToolBar
    {
        public static void DrawToolBar(Rect rect,GUISkin skin)
        {
            GUILayout.BeginHorizontal();
            OuUIUtility.FormatButton("保存", NodeEditor.SaveCurrentCanvas,skin.GetStyle("ToolBarButton"));
            OuUIUtility.FormatButton("另存为", NodeEditor.SaveAs, skin.GetStyle("ToolBarButton"));
            OuUIUtility.FormatButton("加载", NodeEditor.LoadCanvas, skin.GetStyle("ToolBarButton"));
            OuUIUtility.FormatButton("新建", NodeEditor.NewCanvas, skin.GetStyle("ToolBarButton"));
            OuUIUtility.FormatButton("Play", ()=> { EditorApplication.isPlaying = true; } , skin.GetStyle("ToolBarButton"));
            OuUIUtility.FormatButton("End", () => { EditorApplication.isPlaying = false; }, skin.GetStyle("ToolBarButton"));

            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
    }
}
