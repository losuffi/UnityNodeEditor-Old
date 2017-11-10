using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ou.Support.NodeSupport;
using Ou.Support.UnitSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.UnitSupport
{
    public static class UnitEditor
    {
        public static UnitInfo curInfo = new UnitInfo();

        public static void StartInterrupted()
        {
            //Init
            TriggerEditorUtility.Init();
        }

        #region NewOne

        public static void CreateNewUnit()
        {
            UnitField.stateIdentity = "CreateNewOne";
        }

        public static void BuildNewUnit()
        {
            curInfo.curUnit = UnitBase.CreateInstance<UnitBase>();
            curInfo.curUnit.Name = curInfo.unitName;
            Save();
        }

        #endregion

        #region Save

        public static void Save()
        {
            if(curInfo.curUnit==null)
                return;
            if (AssetDatabase.GetAssetPath(curInfo.curUnit).Equals(string.Empty))
            {
                string path = EditorUtility.SaveFilePanel("Save unit", Application.dataPath + "/Ou/Property",
                    curInfo.curUnit.Name, "asset");
                path = Regex.Replace(path, @"^.+/Assets", "Assets");
                AssetDatabase.CreateAsset(curInfo.curUnit, path);
                UnitField.stateIdentity = "showField";
            }
            else
            {
                EditorUtility.SetDirty(curInfo.curUnit);
                AssetDatabase.SaveAssets();
            }
        }

        public static void SaveAs()
        {
            string path = EditorUtility.SaveFilePanel("Save unit", Application.dataPath + "/Ou/Property",
                curInfo.curUnit.Name+"Duplicate", "asset");
            path = Regex.Replace(path, @"^.+/Assets", "Assets");
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(curInfo.curUnit), path);
            AssetDatabase.Refresh();
            var tar = AssetDatabase.LoadAssetAtPath<UnitBase>(path);
            tar.Name = Regex.Match(path, @"/.+?\.", RegexOptions.RightToLeft).Value.TrimStart('/').TrimEnd('.');
            Debug.Log(tar.Name);
            curInfo.curUnit = tar;
            UnitField.stateIdentity = "showField";
        }

        public static void Load()
        {
            string path = EditorUtility.OpenFilePanel("Save unit", Application.dataPath + "/Ou/Property",
                "asset");
            path = Regex.Replace(path, @"^.+/Assets", "Assets");
            Load(path);
        }

        public static void Load(string path)
        {
            var tar = AssetDatabase.LoadAssetAtPath(path, typeof(UnitBase)) as UnitBase;
            if(tar==null)
                return;
            curInfo.curUnit = tar;
            UnitField.stateIdentity = "showField";
        }
        #endregion

        #region ShowField

        public static bool CheckInit()
        {
            if (curInfo.curUnit == null)
                return false;
            return true;
        }

        #endregion

        #region App

        public static void CreateManager()
        {
            var obj = GameObject.Find("_unitManager");
            if (obj == null)
            {
                obj = new GameObject("_unitManager");
                obj.AddComponent<UnitManager>();
            }
        }
        public static void RegisterUnitManager()
        {
            if (GameObject.Find("_nodeTreeManager") == null)
            {
                CreateManager();
            }
            if(curInfo.curUnit==null)
                return;
            GameObject.Find("_unitManager").GetComponent<UnitManager>().registerManager(curInfo.curUnit);
        }
        #endregion
    }
}
