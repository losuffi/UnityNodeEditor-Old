using UnityEditor;
using UnityEngine;
using System;
using Ou.Editor.Windows;

namespace Ou.Editor
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
    }
}
