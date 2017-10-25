using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public static class ConnectionType
    {
        public static Dictionary<string, ConnectionTypeData> types;

        public static void Fetch()
        {
            types=new Dictionary<string, ConnectionTypeData>();
            foreach (Assembly scriptAssembly in AppDomain.CurrentDomain.GetAssemblies().Where(res=>res.FullName.Contains("Assembly-")))
            {
                foreach (Type type in scriptAssembly.GetTypes().Where(res=>!res.IsAbstract&&res.IsClass&&res.GetInterfaces().Contains(typeof(IConnectionDecorator))))
                {
                    IConnectionDecorator icd=scriptAssembly.CreateInstance(type.FullName) as IConnectionDecorator;
                    if (icd != null)
                    {
                        types.Add(icd.identity, new ConnectionTypeData(icd));
                    }
                }
            }
        }

        public static string[] identitys
        {
            get
            {
                string[] strs=new string[types.Count];
                int i = 0;
                foreach (string typesKey in types.Keys)
                {
                    if(types[typesKey].isGlobalVariable)
                        strs[i++] = typesKey;
                }
                return strs;
            }
        }
    }

    public class ConnectionTypeData
    {
        public string identity;
        public Type type;
        public Color color;
        public bool isGlobalVariable;

        public delegate void DelLayout(ref object t);

        public DelLayout GUILayout;
        public Func<object,string> ObjtoString;
        public Func<string, object> StringtoObj;
        internal ConnectionTypeData(IConnectionDecorator icd)
        {
            this.identity = icd.identity;
            this.type = icd.type;
            this.color = icd.color;
            this.isGlobalVariable = icd.isGlobalType;
            this.GUILayout = icd.GUIFill;
            this.ObjtoString = icd.objTostring;
            this.StringtoObj = icd.stringtoobj;
        }
    }

    internal interface IConnectionDecorator
    {
        string identity { get; }
        Type type { get; }
        Color color { get; }
        bool isGlobalType { get; }
        void GUIFill(ref object obj);
        string objTostring(object obj);
        object stringtoobj(string str);
    }

    public class StringType : IConnectionDecorator
    {
        public string identity { get { return "字符串"; } }
        public Type type { get { return typeof(string); } }
        public Color color { get{return Color.cyan;} }
        public bool isGlobalType { get { return true; } }


        public void GUIFill(ref object obj)
        {
            if (obj == null)
                obj = string.Empty;
            obj = GUILayout.TextArea((string)obj);
        }

        public string objTostring(object obj)
        {
            return obj.ToString();
        }

        public object stringtoobj(string str)
        {
            return str;
        }
    }
    public class IntValueType : IConnectionDecorator
    {
        public string identity { get { return "真值"; } }
        public Type type { get { return typeof(int); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }


        public void GUIFill(ref object obj)
        {
            if (!obj.GetType().IsAssignableFrom(typeof(int)))
            {
                obj=new int();
                obj = 0;
            }
            obj = EditorGUILayout.IntField((int) obj);
        }

        public string objTostring(object obj)
        {
            return obj.ToString();
        }

        public object stringtoobj(string str)
        {
            return int.Parse(str);
        }
    }
    public class RealValueType : IConnectionDecorator
    {
        public string identity { get { return "实值"; } }
        public Type type { get { return typeof(float); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }


        public void GUIFill(ref object obj)
        {
            if (!obj.GetType().IsAssignableFrom(typeof(float)))
            {
                obj = new float();
                obj = 0.1f;
            }
            obj = EditorGUILayout.FloatField((float)obj);
        }

        public string objTostring(object obj)
        {
            return obj.ToString();
        }

        public object stringtoobj(string str)
        {
            return float.Parse(str);
        }
    }
    public class WorkstateType : IConnectionDecorator
    {
        public string identity { get { return "工作状态"; } }
        public Type type { get { return typeof(TreeNodeResult);  }}
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return false; } }
        public void GUIFill(ref object obj)
        {
            obj = null;
        }

        public string objTostring(object obj)
        {
            return string.Empty;
        }

        public object stringtoobj(string str)
        {
            return null;
        }
    }
}
