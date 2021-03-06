﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ou.Support.UnitSupport;
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

        public static string UnityUIObjectToString<T>(T obj) where T:Component
        {
            if (obj == null)
                return string.Empty;
            StringBuilder path = new StringBuilder();
            Transform vobj = obj.transform;
            if (vobj.parent == null)
                return "*noObj|" + vobj.name;
            while (true)
            {
                if (vobj.parent==null)
                {
                    path.Remove(0, 1);
                    path.Append("|" + vobj.name);
                    break;
                }
                else
                {
                    path.Insert(0, "/" + vobj.name);
                }
                vobj = vobj.parent;
            }
            return path.ToString();
        }

        public static string UnityUIObjectToString(GameObject obj)
        {
            if (obj == null)
                return string.Empty;
            StringBuilder path = new StringBuilder();
            Transform vobj = obj.transform;
            if (vobj.parent == null)
                return "*noObj|" + vobj.name;
            while (true)
            {
                if (vobj.parent == null)
                {
                    path.Remove(0, 1);
                    path.Append("|" + vobj.name);
                    break;
                }
                else
                {
                    path.Insert(0, "/" + vobj.name);
                }
                vobj = vobj.parent;
            }
            return path.ToString();
        }

        public static T UnityUIStringToObject<T>(string str) where T : Component
        {
            string[] strs = str.Split('|');
            var path = strs[0];
            var root = strs[1];
            if (path.Equals("*noObj"))
                return GameObject.Find(root).GetComponent<T>();
            var gobj = GameObject.Find(root);
            var target = gobj.transform.Find(path);
            if (target == null)
                return null;
            return target.GetComponent<T>();
        }
        public static GameObject UnityUIStringToObject(string str)
        {
            string[] strs = str.Split('|');
            var path = strs[0];
            var root = strs[1];
            if(path.Equals("*noObj"))
                return GameObject.Find(root);
            var gobj = GameObject.Find(root);
            var target = gobj.transform.Find(path);
            if (target == null)
                return null;
            return target.gameObject;
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
            return ConnectionType.UnityUIObjectToString<Text>((Text)obj);
        }

        public object stringtoobj(string str)
        {
            return ConnectionType.UnityUIStringToObject<Text>(str);
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
            if (obj == null)
                return string.Empty;
            return ConnectionType.UnityUIObjectToString<ButtonGroup>((ButtonGroup)obj);
        }

        public object stringtoobj(string str)
        {
            return ConnectionType.UnityUIStringToObject<ButtonGroup>(str);
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
            return ConnectionType.UnityUIObjectToString<Button>((Button)obj);
        }

        public object stringtoobj(string str)
        {
            return ConnectionType.UnityUIStringToObject<Button>(str);
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
            return ConnectionType.UnityUIObjectToString((GameObject)obj);
        }

        public object stringtoobj(string str)
        {
            return ConnectionType.UnityUIStringToObject(str);
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
            return ConnectionType.UnityUIObjectToString<Image>((Image)obj);
        }

        public object stringtoobj(string str)
        {
            return ConnectionType.UnityUIStringToObject<Image>(str);
        }
    }

    public class UGUISprite : IConnectionDecorator
    {
        public string identity { get { return "UISprite"; } }
        public Type type { get { return typeof(Sprite); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "UGUI"; } }

        public void GUIFill(ref object obj)
        {
            try
            {
                obj = EditorGUILayout.ObjectField((Sprite)obj, typeof(Sprite), true);
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
            return AssetDatabase.GetAssetPath(((Sprite)obj));
        }

        public object stringtoobj(string str)
        {
            var obj = AssetDatabase.LoadAssetAtPath<Sprite>(str);
            return obj;
        }
    }
    #endregion

    #region Data

    public class DataCSV : IConnectionDecorator
    {
        public string identity { get { return "CSV"; } }
        public Type type { get { return typeof(string); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "Data"; } }

        public void GUIFill(ref object obj)
        {
            try
            {
                obj = EditorGUILayout.ObjectField((TextAsset)obj, typeof(TextAsset), true);
                if (!objTostring(obj).Contains(".csv"))
                {
                    obj = null;
                }
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
            return AssetDatabase.GetAssetPath(((TextAsset) obj));
        }

        public object stringtoobj(string str)
        {
            var obj = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
            return obj;
        }
    }
    public class DataUnit : IConnectionDecorator
    {
        public string identity { get { return "Unit"; } }
        public Type type { get { return typeof(string); } }
        public Color color { get { return Color.cyan; } }
        public bool isGlobalType { get { return true; } }
        public string _Class { get { return "Data"; } }

        public void GUIFill(ref object obj)
        {
            try
            {
                obj = EditorGUILayout.ObjectField((UnitBase)obj, typeof(UnitBase), true);
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
            var tar = obj as UnitBase;
            return tar.Name;
        }

        public object stringtoobj(string str)
        {
            var manager = GameObject.Find("_unitManager").GetComponent<UnitManager>();
            if (manager == null)
                return null;
            return manager.GetUnit(str);
        }
    }
    #endregion
}
