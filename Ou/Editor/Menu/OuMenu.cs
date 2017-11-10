using UnityEditor;
using UnityEngine;
using System;
using UnityEditor.Callbacks;
using Ou.Support.NodeSupport;
using Ou.Support.UnitSupport;

namespace Ou.Editor.Windows
{
    public partial class OuMenu
    {
        [MenuItem("Ou/UnitEditor")]
        static void UnitEditor()
        {
            UnitEditorWindows.Init();
        }

        [MenuItem("Ou/TriggerEditor")]
        static void TriggerEditor()
        {
            TriggerEditorWindows.Init();
        }
        [OnOpenAsset(1)]
        public static bool AutoOpenAssets(int instanceId, int line)
        {
            if (AssetDatabase.Contains(instanceId))
            {
                string path = AssetDatabase.GetAssetPath(instanceId);
                if (AssetDatabase.LoadAssetAtPath<NodeEditorState>(path)!=null)
                {
                    TriggerEditorWindows.Init();
                    NodeEditor.LoadCanvas(path);
                    return true;
                }
            }
            return false;
        }
        [OnOpenAsset(2)]
        public static bool AutoOpenUnits(int instanceId, int line)
        {
            if (AssetDatabase.Contains(instanceId))
            {
                string path = AssetDatabase.GetAssetPath(instanceId);
                if (AssetDatabase.LoadAssetAtPath<UnitBase>(path) != null)
                {
                    UnitEditorWindows.Init();
                    Support.UnitSupport.UnitEditor.Load(path);
                    return true;
                }
            }
            return false;
        }
    }
}
