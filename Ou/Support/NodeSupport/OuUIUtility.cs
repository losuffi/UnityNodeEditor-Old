using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.UnitSupport;
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
        [HideInInspector]
        public UnitBase unit;

        [HideInInspector] public string[] typeRange;

        public PopupStructer(NodeGraph graph, string[] range)
        {
            if (range.Length != 0)
                this.typeRange = range;
            else
            {
                this.typeRange = ConnectionType.identitys;
            }
            this.datas = graph.selectorVariable(typeRange);
            this.graph = graph;
            unit = null;
        }

        public PopupStructer(string[] range)
        {
            this.datas = null;
            this.graph = null;
            unit = null;
            if (range.Length != 0)
                this.typeRange = range;
            else
            {
                this.typeRange = ConnectionType.identitys;
            }
        }
        public PopupStructer(UnitBase unit,string[] range)
        {
            if (range.Length != 0)
                this.typeRange = range;
            else
            {
                this.typeRange = ConnectionType.identitys;
            }
            this.unit = unit;
            this.datas = unit.selectorVariable(typeRange);
            this.graph = null;            
        }

        public void Update(UnitBase unit, string[] range)
        {
            if (range.Length != 0)
                this.typeRange = range;
            else
            {
                this.typeRange = ConnectionType.identitys;
            }
            this.unit = unit;
            this.datas = unit.selectorVariable(typeRange);
            this.graph = null;
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

        public static Texture2D Tex(Color col)
        {
            if (col == Color.black)
            {
                return AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Ou/OuSource/黑.png");
            }
            else if(col == Color.blue)
            {
                return AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Ou/OuSource/蓝.png");
            }
            else if (col == Color.red)
            {
                return AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Ou/OuSource/红.png");
            }
            else if (col == Color.green)
            {
                return AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Ou/OuSource/绿.png");
            }
            return ColorToTex(5, Color.clear);
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
        public static void FormatSelectedVariableUnit_TypeFit(ref GlobalVariable obj,PopupStructer popupStructer)
        {
            FormatPopup(ref obj.FillIndex, popupStructer.datas);
            var res = popupStructer.datas.Length > 0
                ? popupStructer.datas[obj.FillIndex]
                : string.Empty;
            var duplicate = popupStructer.unit.ReadGlobalVariable(res);
            obj.identity = duplicate.identity;
            obj.name = duplicate.name;
            obj.isFromGlobaldatas = duplicate.isFromGlobaldatas;
            obj.type = duplicate.type;
            obj.obj = duplicate.obj;
            obj.objMessage = duplicate.objMessage;
        }
        public static void FormatFillVariable_SelectedType(ref GlobalVariable obj, ref int index, string name = "temporary", bool isGlobal = false)
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
            OuUIUtility.FormatTextfield(ref obj.name,style);
            OuUIUtility.FormatLabel(obj.identity);
            OuUIUtility.FormatFillVariable_SelectedType(ref obj, ref obj.FillIndex, obj.name, true);
        }
        public static void FormatShowVariable_Exits(ref GlobalVariable obj, GUIStyle style,GUIStyle style2)
        {
            OuUIUtility.FormatLabel("属性名", style2);
            OuUIUtility.FormatTextfield(ref obj.name, style);
            OuUIUtility.FormatFillVariable_SelectedType(ref obj, ref obj.FillIndex, obj.name, true);
        }
        public static void FormatShowVariable_Exits(ref GlobalVariable obj)
        {
            OuUIUtility.FormatTextfield(ref obj.name);
           // OuUIUtility.FormatLabel(obj.identity);
            OuUIUtility.FormatFillVariable_SelectedType(ref obj, ref obj.FillIndex, obj.name, true);
        }
        #endregion
        #region FunctionalUI


        public static void DrawLine(Vector3 startPos, Vector3 endPos)
        {
            Vector3 startTan = startPos + Vector3.up * 50;
            Vector3 endTan = endPos + Vector3.down * 50;
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.white, null, 5);
        }

        public static void DrawLineA(Vector3 startPos, Vector3 endPos)
        {
            Vector3[] Points=new Vector3[4];
            Points[0] = startPos;
            Points[3] = endPos;
            if (startPos.y < endPos.y)
            {
                Points[1] = new Vector3(startPos.x, startPos.y + 10);
                Points[2] = new Vector3(endPos.x, Points[1].y);
            }
            else
            {
                float xMins = startPos.x - endPos.x;
                float yMins = startPos.y - endPos.y;
                Points[1] = new Vector3(startPos.x - 0.2f * xMins, startPos.y);
                Points[2] = new Vector3(Points[1].x, endPos.y);
            }
            Handles.DrawAAPolyLine(5,Points);
        }


        public static void FormatPopup(ref int index, params string[] strs)
        {
            if (strs == null)
            {
                EditorGUILayout.Popup(index, new string[0],GUILayout.Width(60));
            }
            else
            {
                index = EditorGUILayout.Popup(index, strs, GUILayout.Width(60));
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
            TreeNodeReflectionSystem.Fetch();
            NodeEditor.CreateManager();
            UnitEditor.CreateManager();
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

        #region algorithm

        public static string MathString_InfixToPostfix(string input)
        {
            Stack<char> operators = new Stack<char>();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                if (char.IsWhiteSpace(ch)) continue;
                switch (ch)
                {
                    case '+':
                    case '-':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();   //pop Operator
                            if (c == '(')
                            {
                                operators.Push(c);      //push Operator
                                break;
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        operators.Push(ch);
                        result.Append(" ");
                        break;
                    case '*':
                    case '/':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();
                            if (c == '(')
                            {
                                operators.Push(c);
                                break;
                            }
                            else
                            {
                                if (c == '+' || c == '-')
                                {
                                    operators.Push(c);
                                    break;
                                }
                                else
                                {
                                    result.Append(c);
                                }
                            }
                        }
                        operators.Push(ch);
                        result.Append(" ");
                        break;
                    case '(':
                        operators.Push(ch);
                        break;
                    case ')':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();
                            if (c == '(')
                            {
                                break;
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        break;
                    default:
                        result.Append(ch);
                        break;
                }
            }
            while (operators.Count > 0)
            {
                result.Append(operators.Pop()); //pop All Operator
            }
            return result.ToString();
        }

        public static double MathString_Parse(string expression)
        {
            string postfixExpression = MathString_InfixToPostfix(expression);
            Stack<double> results = new Stack<double>();
            double x, y;
            for (int i = 0; i < postfixExpression.Length; i++)
            {
                char ch = postfixExpression[i];
                if (char.IsWhiteSpace(ch)) continue;
                switch (ch)
                {
                    case '+':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x + y);
                        break;
                    case '-':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x - y);
                        break;
                    case '*':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x * y);
                        break;
                    case '/':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x / y);
                        break;
                    default:
                        int pos = i;
                        StringBuilder operand = new StringBuilder();
                        do
                        {
                            operand.Append(postfixExpression[pos]);
                            pos++;
                        } while (char.IsDigit(postfixExpression[pos]) || postfixExpression[pos] == '.');
                        i = --pos;
                        results.Push(double.Parse(operand.ToString()));
                        break;
                }
            }
            return results.Peek();
        }
        #endregion
    }
}
