using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.UnitSupport
{
    public class UnitManager:MonoBehaviour
    {
        public List<UnitBase> units = new List<UnitBase>();
        void Start()
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i] == null)
                {
                    units.RemoveAt(i);
                    i--;
                    continue;
                }
                units[i].InitialzationVariable();
            }
        }

        void OnApplicationQuit()
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].EndRuntimeVariable();
            }
        }

        public void registerManager(UnitBase unit)
        {
            if(unit==null||units.Contains(unit))
                return;
            units.Add(unit);
        }

        public UnitBase GetUnit(string key)
        {
            return units.Find(res => res.Name.Equals(key));
        }
    }
}
