using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public class TreeNodeGUIManager:MonoBehaviour
    {
        public delegate void GUIManagerFunc(Event e);

        public List<KeyValuePair<string, GUIManagerFunc>> guiManagerFuncs = new List<KeyValuePair<string, GUIManagerFunc>>();

        public void Bind(string funcName,GUIManagerFunc func)
        {
            guiManagerFuncs.Add(new KeyValuePair<string, GUIManagerFunc>(funcName, func));
        }

        public void Remove(string funcName)
        {
            var result = guiManagerFuncs.Find(res => res.Key.Equals(funcName));
            if (result.Value == null)
            {
                return;
            }
            guiManagerFuncs.Remove(result);
        }
        void OnGUI()
        {
            if (guiManagerFuncs.Any())
            {
                for (int i = 0; i < guiManagerFuncs.Count; i++)
                {
                    guiManagerFuncs[i].Value(Event.current);
                }
            }
        }
    }
}
