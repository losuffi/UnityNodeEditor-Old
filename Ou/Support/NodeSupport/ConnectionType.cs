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

        public delegate void DelLayout(out object t);

        public DelLayout GUILayout;
        internal ConnectionTypeData(IConnectionDecorator icd)
        {
            this.identity = icd.identity;
            this.type = icd.type;
            this.color = icd.color;
            this.isGlobalVariable = icd.isGlobalType;
            this.GUILayout = icd.GUIFill;
        }
    }

    internal interface IConnectionDecorator
    {
        string identity { get; }
        Type type { get; }
        Color color { get; }
        bool isGlobalType { get; }
        void GUIFill(out object obj);
    }

    public class StringType : IConnectionDecorator
    {
        public string identity { get { return "字符串"; } }
        public Type type { get { return typeof(string); } }
        public Color color { get{return Color.cyan;} }
        public bool isGlobalType { get { return true; } }
        public void GUIFill(out object obj)
        {
            obj = string.Empty;
            obj = GUILayout.TextArea(obj.ToString());
        }
    }
    public class IntValueType : IConnectionDecorator
    {
        public string identity { get { return "真值"; } }
        public Type type { get { return typeof(int); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public void GUIFill(out object obj)
        {
            obj=new int();
            obj = 0;
            obj = EditorGUILayout.IntField((int) obj);
        }
    }
    public class RealValueType : IConnectionDecorator
    {
        public string identity { get { return "实值"; } }
        public Type type { get { return typeof(float); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public void GUIFill(out object obj)
        {
            obj=new float();
            obj = 0.1f;
            obj = EditorGUILayout.FloatField((float)obj);
        }
    }
    public class WorkstateType : IConnectionDecorator
    {
        public string identity { get { return "工作状态"; } }
        public Type type { get { return typeof(TreeNodeResult);  }}
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return false; } }
        public void GUIFill(out object obj)
        {
            obj = null;
        }
    }
}
