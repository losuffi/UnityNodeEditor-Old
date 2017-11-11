using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ou.Support.UnitSupport;

namespace Ou.Support.NodeSupport
{
    public static class TreeNodeReflectionSystem
    {
        public static List<KeyValuePair<NodeMethodAttribute,Delegate>> methods=new List<KeyValuePair<NodeMethodAttribute, Delegate>>();
        public static void Fetch()
        {
            IEnumerable<Assembly> scriptAssemblies =
                AppDomain.CurrentDomain.GetAssemblies().Where(ar => ar.FullName.Contains("Assembly-"));
            foreach (Assembly assembly in scriptAssemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Static))
                    {
                        foreach (object attribute in method.GetCustomAttributes(true))
                        {
                            Type attributeType = attribute.GetType();
                            if (attributeType == typeof(NodeMethodAttribute))
                            {
                                methods.Add(new KeyValuePair<NodeMethodAttribute, Delegate>(
                                    attribute as NodeMethodAttribute,
                                    Delegate.CreateDelegate(typeof(Func<UnitBase,bool>), method)));
                            }
                        }
                    }
                }
            }
        }

        public static Delegate GetMethod(string key)
        {
            return methods.Find(res => res.Key.identity.Equals(key)).Value;
        }

        public static string[] methodNames
        {
            get
            {
                string[] strs = new string[methods.Count];
                int i = 0;
                foreach (var typesKey in methods)
                {
                    strs[i] = typesKey.Key.identity;
                    i++;
                }
                return strs;
            }
        }
     }
    public class NodeMethodAttribute : Attribute
    {
        public string identity { get; private set; }


        public NodeMethodAttribute(string id)
        {
            identity = id;
        }
    }
}
