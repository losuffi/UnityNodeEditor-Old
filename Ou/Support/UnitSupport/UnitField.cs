using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using UnityEngine;

namespace Ou.Support.UnitSupport
{
    public static class UnitField
    {
        public static UnitBase curUnit = null;
        public static string stateIdentity = "none";

        public static void DrawField(GUISkin skin)
        {
            if (stateIdentity.Equals("CreateNewOne"))
            {
                CreateNewOne(skin);
            }
            if (stateIdentity.Equals("showField"))
            {
                ShowField(skin);
            }
        }

        static void CreateNewOne(GUISkin skin)
        {
            #region Handle

            GUILayout.BeginHorizontal();
            OuUIUtility.FormatLabel("Unit名(非中文)：", skin.GetStyle("UnitEditorFieldLabel"));
            OuUIUtility.FormatTextfield(ref UnitEditor.curInfo.unitName, skin.GetStyle("UnitEditorFieldField"));
            OuUIUtility.FormatButton("建立", UnitEditor.BuildNewUnit);
            GUILayout.EndHorizontal();
            #endregion
        }

        static void ShowField(GUISkin skin)
        {
            GUILayout.BeginVertical();
            if (UnitEditor.CheckInit())
            {
                curUnit = UnitEditor.curInfo.curUnit;
                for (int i = 0; i < curUnit.fields.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    var variable = curUnit.fields[i];
                    OuUIUtility.FormatShowVariable_Exits(ref variable, skin.GetStyle("UnitEditorFieldLabel"));
                    OuUIUtility.FormatButton("-", () => { curUnit.fields.Remove(variable);
                        i--;
                    });
                    GUILayout.EndHorizontal();
                }
                OuUIUtility.FormatButton("添加属性",AddField);
            }
            GUILayout.EndVertical();
        }

        static void AddField()
        {
            GlobalVariable obj=new GlobalVariable();
            //TODO:增加GV 的Draw
        }
    }
}
