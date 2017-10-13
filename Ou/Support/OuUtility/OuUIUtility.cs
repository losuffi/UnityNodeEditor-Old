using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.Node;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.OuUtility
{
    public static class OuUIUtility
    {
        public static GUISkin GetGUISkinStyle(string skinName)
        {
            return AssetDatabase.LoadAssetAtPath<GUISkin>(@"Assets/Ou/GUI Skin/Editor/" + skinName + ".guiskin");
        }
    }

    public static class TriggerEditorUtility
    {
        public static void Init()
        {
            NodeTypes.FetchNode();
            NodeInputSystem.Fetch();
        }
    }
}
