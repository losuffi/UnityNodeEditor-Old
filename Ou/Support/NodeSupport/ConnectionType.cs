using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
        string _Class { get; }
    }

    #region Normal

    public class StringType : IConnectionDecorator
    {
        public string identity { get { return "字符串"; } }
        public Type type { get { return typeof(string); } }
        public Color color { get{return Color.cyan;} }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "Normal"; } }

        public void GUIFill(ref object obj)
        {
            if (obj==null||!obj.GetType().IsAssignableFrom(typeof(string)))
            {
                obj = string.Empty;
            }
            if (GUILayout.Button("粘贴", GUILayout.Width(40)))
            {
                obj = EditorGUIUtility.systemCopyBuffer;
            }
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
        public string _Class { get { return "Normal"; } }

        public void GUIFill(ref object obj)
        {
            if (obj == null || !obj.GetType().IsAssignableFrom(typeof(int)))
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
            int res;

            if (int.TryParse(str, out res))
            {
                return res;
            }
            return null;
        }
    }
    public class RealValueType : IConnectionDecorator
    {
        public string identity { get { return "实值"; } }
        public Type type { get { return typeof(float); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "Normal"; } }

        public void GUIFill(ref object obj)
        {
            if (obj == null || !obj.GetType().IsAssignableFrom(typeof(float)))
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
            float res;
            if (float.TryParse(str, out res))
            {
                return res;
            }
            return null;
        }
    }
    public class WorkstateType : IConnectionDecorator
    {
        public string identity { get { return "工作状态"; } }
        public Type type { get { return typeof(TreeNodeResult);  }}
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return false; } }
        public string _Class { get { return "Normal"; } }
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



    #endregion



    #region UGUI
    public class UGUITextType : IConnectionDecorator
    {
        public string identity { get { return "TextUI"; } }
        public Type type { get { return typeof(string); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "UGUI"; } }

        public void GUIFill(ref object obj)
        {
            try
            {
                obj = EditorGUILayout.ObjectField((Text)obj, typeof(Text), true);
            }
            catch (InvalidCastException e)
            {
                obj = null;
            }
        }

        public string objTostring(object obj)
        {
            if (obj == null)
                return string.Empty;
            return ((Text) obj).name;
        }

        public object stringtoobj(string str)
        {
            var gobj = GameObject.Find(str);
            if (gobj == null)
                return null;
            return GameObject.Find(str).GetComponent<Text>();
        }
    }

    public class UGUIButtonGroup : IConnectionDecorator
    {
        public string identity { get { return "ButtonGroupUI"; } }
        public Type type { get { return typeof(ButtonGroup); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "UGUI"; } }

        public void GUIFill(ref object obj)
        {
            try
            {
                obj = EditorGUILayout.ObjectField((ButtonGroup)obj, typeof(ButtonGroup), true);
            }
            catch (InvalidCastException e)
            {
                obj = null;
            }
        }

        public string objTostring(object obj)
        {
            return ((ButtonGroup)obj).name;
        }

        public object stringtoobj(string str)
        {
            var gobj = GameObject.Find(str);
            if (gobj == null)
                return null;
            return GameObject.Find(str).GetComponent<ButtonGroup>();
        }
    }

    public class UGUIButtonType : IConnectionDecorator
    {
        public string identity { get { return "ButtonUI"; } }
        public Type type { get { return typeof(Button); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "UGUI"; } }

        public void GUIFill(ref object obj)
        {
            try
            {
                obj = EditorGUILayout.ObjectField((Button)obj, typeof(Button), true);
            }
            catch (InvalidCastException e)
            {
                obj = null;
            }
        }

        public string objTostring(object obj)
        {
            if (obj == null)
                return string.Empty;
            return ((Button)obj).name;
        }

        public object stringtoobj(string str)
        {
            var gobj = GameObject.Find(str);
            if (gobj == null)
                return null;
            return GameObject.Find(str).GetComponent<Button>();
        }
    }

    public class UGUIObjectType : IConnectionDecorator
    {
        public string identity { get { return "ObjectUI"; } }
        public Type type { get { return typeof(GameObject); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "UGUI"; } }

        public void GUIFill(ref object obj)
        {
            try
            {
                obj = EditorGUILayout.ObjectField((GameObject)obj, typeof(GameObject), true);
            }
            catch (InvalidCastException e)
            {
                obj = null;
            }
        }

        public string objTostring(object obj)
        {
            if (obj == null)
                return string.Empty;
            return ((GameObject)obj).name;
        }

        public object stringtoobj(string str)
        {
            var gobj = GameObject.Find(str);
            if (gobj == null)
                return null;
            return gobj;
        }
    }
    public class UGUIImageType : IConnectionDecorator
    {
        public string identity { get { return "ImageUI"; } }
        public Type type { get { return typeof(Image); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "UGUI"; } }

        public void GUIFill(ref object obj)
        {
            try
            {
                obj = EditorGUILayout.ObjectField((Image)obj, typeof(Image), true);
            }
            catch (InvalidCastException e)
            {
                obj = null;
            }
        }

        public string objTostring(object obj)
        {
            if (obj == null)
                return string.Empty;
            return ((Image)obj).name;
        }

        public object stringtoobj(string str)
        {
            var gobj = GameObject.Find(str);
            if (gobj == null)
                return null;
            return GameObject.Find(str).GetComponent<Image>();
        }
    }
    #endregion
}
