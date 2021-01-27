using System;
using System.Collections.Generic;
using UnityEngine;

namespace vitamin
{
    public enum UIType { FIX, DIALOG, FLOAT, ALERT };

    public class UIManager : MonoBehaviour
    {
        private FairyGUI.GComponent FixContainer;
        private FairyGUI.GComponent DialogContainer;
        private FairyGUI.GComponent FloatContainer;
        private FairyGUI.GComponent AlertContainer;
        private FairyGUI.GComponent TipContainer;
        private Dictionary<Type, ViewFairy> map;
        private List<ViewFairy> openlist;
        private string DefaultUIPackName;

        internal EventEmitter _emitter;

  
            
        public void initialize()
        {
            this.map = new Dictionary<Type, ViewFairy>();
            this.openlist = new List<ViewFairy>();
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

        /// <summary>
        /// 加载资源包
        /// </summary>
        /// <returns></returns>
        public void Load(string path)
        {
            FairyGUI.UIPackage.AddPackage(path);
            if(DefaultUIPackName==null) DefaultUIPackName = path.LastIndexOf("/") >= 0 ? path.Substring(path.LastIndexOf("/"), path.Length) : path;
        }

        /// <summary>
        /// 注册界面
        /// </summary>
        /// <param name="uitype">UI显示类型</param>
        /// <param name="uiresname">UI资源名称,默认为UIType的类名称</param>
        /// <param name="respackname">UI所属资源包,默认为加载包名</param>
        /// <returns></returns>
        public void Register<T>(UIType uitype, string uiresname=null, string respackname = null) where T: ViewFairy
        {
            Type viewType = typeof(T);
            if(uiresname==null){
                uiresname=viewType.Name;
                if(uiresname.IndexOf(".")>0){
                    uiresname=uiresname.Substring(uiresname.IndexOf('.')+1);
                }
            }
            Logger.Log(uiresname);
            object[] args = { uiresname, respackname != null ? respackname : DefaultUIPackName, uitype };
            
            T view = Injector.createView<T>(args);
            view._emitter = _emitter;
            this.map[viewType] = view;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <returns></returns>
        public T Open<T>() where T : ViewFairy
        {
            Type ViewClazz = typeof(T);
            //T FindObjectOfType<T>() where T : Object;
            ViewFairy view = map[ViewClazz];
            if (view == null) return null;
            if (this.openlist.IndexOf(view) >= 0) return null;
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
            this.openlist.Add(view);
            view.enter();
            view.Resize(FairyGUI.GRoot.inst.width, FairyGUI.GRoot.inst.height);
            return (T)view;
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <returns></returns>
        public void Close<T>() where T : ViewFairy
        {
            Type ViewClazz = typeof(T);
            ViewFairy view = map[ViewClazz];
            if (view == null) return;
            int index = this.openlist.IndexOf(view);
            if (index >= 0)
            {
                this.openlist.RemoveAt(index);
                view.Remove();
                view.exit();
            }
        }

        internal void Resize()
        {
            foreach(ViewFairy view in this.openlist)
            {
                view.Resize(FairyGUI.GRoot.inst.width, FairyGUI.GRoot.inst.height);
            }
        }

        public T Get<T>() where T : ViewFairy
        {
            Type ViewClazz = typeof(T);
            return (T)this.map[ViewClazz];
        }
    }
}