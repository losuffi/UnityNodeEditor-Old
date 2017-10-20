using System;
using Ou.Support.NodeSupport;
using UnityEditor;
using UnityEngine;

namespace Ou.Editor.Views
{
    [Serializable]
    public  class ViewBase
    {
        public string Title;
        public Rect ViewRect;
        protected GUISkin ViewSkin;

        public ViewBase()
        {
            Title=String.Empty;
            ViewRect=new Rect();
            GetGUISkin();
        }

        private void GetGUISkin()
        {
            ViewSkin = AssetDatabase.LoadAssetAtPath<GUISkin>(@"Assets/Ou/GUI Skin/Editor/NormalSkin.guiskin");
        }

        public ViewBase(string title)
        {
            Title = title;
            ViewRect = new Rect();
            GetGUISkin();
        }

        public ViewBase(string title, Rect viewRect)
        {
            Title = title;
            ViewRect = viewRect;
            GetGUISkin();
        }

        public virtual void UpdateView(Rect size, Rect percentageSize, Event e)
        {
            if(ViewSkin==null)
                GetGUISkin();
            ViewRect=new Rect(size.x*percentageSize.x,
                              size.y*percentageSize.y,
                              size.width*percentageSize.width,
                              size.height*percentageSize.height);
        }
        public virtual void UpdateView(Rect size, Rect percentageSize, Event e,NodeGraph currentNode)
        {
            if (ViewSkin == null)
                GetGUISkin();
            ViewRect = new Rect(size.x * percentageSize.x,
                size.y * percentageSize.y,
                size.width * percentageSize.width,
                size.height * percentageSize.height);
        }
        public virtual void ProcessEvent(Event e)
        {
            
        }
    }
}
