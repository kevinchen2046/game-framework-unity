using System;
using System.Collections.Generic;

namespace vitamin
{
    public enum UIType { FIX, DIALOG, FLOAT, ALERT };

    public class UIManager : EventEmitter
    {
        private FairyGUI.GComponent FixContainer;
        private FairyGUI.GComponent DialogContainer;
        private FairyGUI.GComponent FloatContainer;
        private FairyGUI.GComponent AlertContainer;
        private FairyGUI.GComponent TipContainer;
        private Dictionary<Type, ViewFairy> map;
        private string DefaultUIPackName;

        public UIManager()
        {
            this.map = new Dictionary<Type, ViewFairy>();
            this.FixContainer = new FairyGUI.GComponent();
            FairyGUI.GRoot.inst.AddChild(this.FixContainer);
            this.DialogContainer = new FairyGUI.GComponent();
            FairyGUI.GRoot.inst.AddChild(this.DialogContainer);
            this.FloatContainer = new FairyGUI.GComponent();
            FairyGUI.GRoot.inst.AddChild(this.FloatContainer);
            this.AlertContainer = new FairyGUI.GComponent();
            FairyGUI.GRoot.inst.AddChild(this.AlertContainer);
            this.TipContainer = new FairyGUI.GComponent();
            FairyGUI.GRoot.inst.AddChild(this.TipContainer);
        }
        public void Load(string path)
        {
            FairyGUI.UIPackage.AddPackage(path);
            DefaultUIPackName = path.LastIndexOf("/") >= 0 ? path.Substring(path.LastIndexOf("/"), path.Length) : path;
        }

        public void Register(UIType uitype, Type ViewClazz, string uiresname, string respackname = null)
        {
            object[] args = { uiresname, respackname != null ? respackname : DefaultUIPackName, uitype };
            this.map[ViewClazz] = Activator.CreateInstance(ViewClazz, args) as ViewFairy;
        }

        public T Open<T>() where T : ViewFairy
        {
            Type ViewClazz = typeof(T);
            //T FindObjectOfType<T>() where T : Object;
            ViewFairy view = map[ViewClazz];
            if (view != null)
            {
                switch (view.uitype)
                {
                    case UIType.FIX:
                        view.Add(this.FixContainer);
                        break;
                    case UIType.DIALOG:
                        view.Add(this.DialogContainer);
                        break;
                    case UIType.FLOAT:
                        view.Add(this.FloatContainer);
                        break;
                    case UIType.ALERT:
                        view.Add(this.AlertContainer);
                        break;
                }
            }
            view.enter();
            return (T)view;
        }

        public T Get<T>() where T : ViewFairy
        {
            Type ViewClazz = typeof(T);
            return (T)this.map[ViewClazz];
        }
    }
}