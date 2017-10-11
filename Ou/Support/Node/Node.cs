using UnityEngine;
using System;
using UnityEditor;
namespace Ou.NodeEditor
{
    public abstract class BasicNode
    {
        public string Title;
        public EditorWindow editorwindow;
        protected abstract void Evaluator();
        public virtual void Init()
        {

        }
    }
}
