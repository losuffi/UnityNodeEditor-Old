using UnityEditor;
using UnityEngine;
using System;
using UnityEditor.Callbacks;
using Ou.Support.NodeSupport;
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
    }
}
