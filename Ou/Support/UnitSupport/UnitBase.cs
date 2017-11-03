using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ou.Support.NodeSupport;

namespace Ou.Support.UnitSupport
{
    [System.Serializable]
    public class UnitBase : ScriptableObject
    {
        public int ID;
        public string Name;
        [SerializeField] protected internal List<GlobalVariable> fields = new List<GlobalVariable>();
        private List<GlobalVariable> fieldsRuntime = new List<GlobalVariable>();

        public void InitialzationVariable()
        {
            fieldsRuntime.Clear();
            for (int gvCount = 0; gvCount < fields.Count; gvCount++)
            {
                var tar = new GlobalVariable(fields[gvCount]);
                tar.ConvertObject();
                fieldsRuntime.Add(tar);
            }
        }

        public void EndRuntimeVariable()
        {
            if (!fieldsRuntime.Any())
            {
                return;
            }
            fields.Clear();
            for (int gvCount = 0; gvCount < fieldsRuntime.Count; gvCount++)
            {
                fields.Add(fieldsRuntime[gvCount]);
            }
            fieldsRuntime.Clear();
        }

        public string[] selectorVariable(params string[] id)
        {
            List<string> res = new List<string>();
            for (int j = 0; j < fields.Count; j++)
            {
                foreach (string s in id)
                {
                    if (fields[j].identity.Equals(s))
                    {
                        res.Add(fields[j].name);
                        break;
                    }
                }
            }
            return res.ToArray();
        }

        #region Handle Variable

        public GlobalVariable ReadGlobalVariable(string key)
        {
            if (CheckKey(key))
            {
                var tar = fields.Find(z => z.name.Equals(key));
                return tar;
            }
            return new GlobalVariable(typeof(string), string.Empty, "字符串", "none");
        }

        public GlobalVariable ReadGlobalVariable(int index)
        {
            if (fields.Count > index)
            {
                var tar = fields[index];
                return tar;
            }
            return new GlobalVariable(typeof(string), string.Empty, "字符串", "none");
        }

        public bool CheckKey(string key)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].name.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateGlobalVarible(string key, object obj)
        {
            if (CheckKey(key))
            {
                var tar = fields.Find(z => z.name.Equals(key));
                tar.obj = obj;
                tar.ConvertString();
            }
        }

        public void RemoveGlobalVariable(int index)
        {
            fields.RemoveAt(index);
        }

        #endregion
    }
}
