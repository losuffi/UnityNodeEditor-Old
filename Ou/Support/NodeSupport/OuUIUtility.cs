using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Serializable]
    public class PopupStructer
    {
        [HideInInspector]
        public string[] datas;
        [HideInInspector]
        public NodeGraph graph;

        [HideInInspector] public string[] typeRange;

        public PopupStructer(string[] range,NodeGraph graph)
        {
            this.datas = graph.selectorVariable(range);
            this.graph = graph;
            if(range.Length!=0)
                this.typeRange = range;
            else
            {
                this.typeRange = ConnectionType.identitys;
            }
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

        #region GlobalVariable
        public static void FormatSelectedVariable_TypeFit(ref GlobalVariable obj, ref int index, PopupStructer popupStructer)
        {
            FormatPopup(ref index, popupStructer.datas);
            var res = popupStructer.datas.Length > 0
                ? popupStructer.datas[index]
                : string.Empty;
            var duplicate = popupStructer.graph.ReadGlobalVariable(res);
            obj.identity = duplicate.identity;
            obj.name = duplicate.name;
            obj.isFromGlobaldatas = duplicate.isFromGlobaldatas;
            obj.type = duplicate.type;
            obj.obj = duplicate.obj;
            obj.objMessage = duplicate.objMessage;
        }
        public static void FormatSetVariable_SelectedType(ref GlobalVariable obj, ref int index, string name = "temporary", bool isGlobal = false)
        {
            FormatPopup(ref index, obj.structerTypeRange.typeRange);
            var icd = ConnectionType.types[obj.structerTypeRange.typeRange[index]];
            obj.name = name;
            obj.type = icd.type;
            obj.identity = icd.identity;
            ConnectionType.types[obj.structerTypeRange.typeRange[index]].GUILayout(ref obj.obj);
            obj.isFromGlobaldatas = isGlobal;
        }

        public static void FormatShowVariable_Exits(ref GlobalVariable obj,GUIStyle style)
        {
            OuUIUtility.FormatLabel(obj.name,style);
            OuUIUtility.FormatLabel(obj.identity,style);
            ConnectionType.types[obj.identity].GUILayout(ref obj.obj);
        }
        #endregion
        #region FunctionalUI


        public static void DrawLine(Vector3 startPos, Vector3 endPos)
        {
            Vector3 startTan = startPos + Vector3.up * 50;
            Vector3 endTan = endPos + Vector3.down * 50;
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.white, null, 5);
        }



        public static void FormatPopup(ref int index, params string[] strs)
        {
            if (strs == null)
            {
                EditorGUILayout.Popup(index, new string[0]);
            }
            else
            {
                index = EditorGUILayout.Popup(index, strs);
            }
        }


        public static void FormatLabel(string str)
        {
            GUILayout.Label(str);
        }
        public static void FormatLabel(string str,GUIStyle style)
        {
            GUILayout.Label(str,style);
        }

        public static void FormatTextfield(ref string str)
        {
            if (GUILayout.Button("粘贴", GUILayout.Width(40)))
            {
                str = EditorGUIUtility.systemCopyBuffer;
            }
            str = GUILayout.TextField(str);
        }

        public static void FormatTextfield(ref string str, GUIStyle style)
        {
            if (GUILayout.Button("粘贴", GUILayout.Width(40)))
            {
                str = EditorGUIUtility.systemCopyBuffer;
            }
            str = GUILayout.TextField(str,style);
        }

        public static void FormatIntfield(ref int val)
        {
            val = EditorGUILayout.IntField(val);
        }
        public static void FormatTextArea(ref string str)
        {
            if (GUILayout.Button("粘贴", GUILayout.Width(40)))
            {
                str = EditorGUIUtility.systemCopyBuffer;
            }
            str = GUILayout.TextArea(str);
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
