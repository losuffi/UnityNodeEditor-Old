using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Serializable]
    public struct PopupStructer
    {
        [HideInInspector]
        public string[] datas;
        [HideInInspector]
        public int selectionIndex;
        [HideInInspector]
        public string value;

        public PopupStructer(string[] datas)
        {
            this.datas = datas;
            this.selectionIndex = 0;
            this.value = datas.Length > 0 ? datas[0] : string.Empty;
        }

    }
    public static class OuUIUtility
    {


        #region FuncNormal

        public static GUISkin GetGUISkinStyle(string skinName)
        {
            return AssetDatabase.LoadAssetAtPath<GUISkin>(@"Assets/Ou/GUI Skin/Editor/" + skinName + ".guiskin");
        }

        #endregion
        #region Low-Level UI

        public static Texture2D ColorToTex(int pxSize, Color col)
        {
            Texture2D tex = new Texture2D(pxSize, pxSize);
            for (int x = 0; x < pxSize; x++)
                for (int y = 0; y < pxSize; y++)
                    tex.SetPixel(x,y,col);
            tex.Apply();
            return tex;
        }

        #endregion

        #region FunctionalUI


        public static void DrawLine(Vector3 startPos, Vector3 endPos)
        {
            Vector3 startTan = startPos + Vector3.up * 50;
            Vector3 endTan = endPos + Vector3.down * 50;
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.white, null, 5);
        }

        public static void FormatSetType(SettingType type,ref string fill,ref PopupStructer popupStructer)
        {
            if (type == SettingType.填充)
            {
                GUILayout.Label("填充所需变量");
                fill = GUILayout.TextField(fill);
            }
            else
            {
                GUILayout.Label("选择全局变量");
                popupStructer.selectionIndex =
                    EditorGUILayout.Popup(popupStructer.selectionIndex, popupStructer.datas);
                popupStructer.value = popupStructer.datas.Length > 0
                    ? popupStructer.datas[popupStructer.selectionIndex]
                    : string.Empty;
            }
        }

        public static void FormatVariableType(ref GlobalVariable obj,ref int index)
        {
            index = EditorGUILayout.Popup(index, ConnectionType.identitys);
            var icd = ConnectionType.types[ConnectionType.identitys[index]];
            obj.type = icd.type;
            obj.identity = icd.identity;
            obj.Update();
            ConnectionType.types[ConnectionType.identitys[index]].GUILayout(ref obj.obj);

        }
        public static void FormatTextfield(ref string str)
        {
            str = GUILayout.TextField(str);
        }
        public static void FormatTextfield(ref string str,GUIStyle style)
        {
            str = GUILayout.TextField(str, style);
        }

        public static void FormatButton(string label, Action method,GUIStyle style=null)
        {
            if (style == null)
            {
                if (GUILayout.Button(label))
                {
                    method();
                }
            }
            else
            {
                if (GUILayout.Button(label, style))
                {
                    method();
                }
            }
        }
        public static void FormatButton(string label, Action method, Rect rect, GUIStyle style = null)
        {
            if (style == null)
            {
                if (GUI.Button(rect, label))
                {
                    method();
                }
            }
            else
            {
                if (GUI.Button(rect, label, style))
                {
                    method();
                }
            }
        }
        public static void FormatButton(string label, Action method, Vector2 size, GUIStyle style = null)
        {
            if (style == null)
            {
                if (GUILayout.Button(label, GUILayout.Width(size.x), GUILayout.Height(size.y)))
                {
                    method();
                }
            }
            else
            {
                if (GUILayout.Button(label, style, GUILayout.Width(size.x), GUILayout.Height(size.y)))
                {
                    method();
                }
            }
        }
        #endregion

    }

    public static class TriggerEditorUtility
    {
        public static bool IsInit = false;
        public static bool IsLayout = false;
        public static event Action TrigInit; 

        public static Event e;
        public static void Init()
        {
            IsInit = false;
            NodeTypes.FetchNode();
            NodeInputSystem.Fetch();
            ConnectionType.Fetch();
            NodeEditor.CreateManager();
            IsInit = true;
            if (TrigInit != null)
                TrigInit();
        }

        public static void ProcessE(Event e)
        {
            TriggerEditorUtility.e = e;
        }
        public static bool CheckInit()
        {
            return IsInit && NodeEditor.curNodeEditorState != null && NodeEditor.curNodeGraph != null;
        }

        public static bool LayoutCheck()
        {
            if ((IsLayout && e.type != EventType.Layout) || e.type == EventType.Layout)
            {
                IsLayout = true;
                return true;
            }
            return false;
        }
    }
}
