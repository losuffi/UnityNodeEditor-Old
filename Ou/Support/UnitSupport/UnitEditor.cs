using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            //ReStart
        }

        #region NewOne

        public static void CreateNewUnit()
        {
            UnitField.stateIdentity = "CreateNewOne";
        }

        public static void BuildNewUnit()
        {
            curInfo.curUnit = UnitBase.CreateInstance<UnitBase>();
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
                    "DefaultUnit", "asset");
                path = Regex.Replace(path, @"^.+/Assets", "Assets");
                AssetDatabase.CreateAsset(curInfo.curUnit, path);
                UnitField.stateIdentity = "showField";
            }
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
    }
}
