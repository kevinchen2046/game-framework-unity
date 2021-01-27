using vitamin;
using game.view;
using UnityEngine;
using System.Text.RegularExpressions;

namespace game { 
    public class UI : Context
    {
        [Tooltip("多个资源请用,号隔开")]
        [CustomInspectorAttribute("资源包")]
        public string respack= "game";

        override internal void initialize()
        {
            //解析包名称
            string[] list = Regex.Split(respack,",");
            foreach(var res in list){
                 Vitamin.inst.ui.Load(res);
            }
            //注册界面
            Vitamin.inst.ui.Register<MainView>(vitamin.UIType.FIX);
            
            //打开界面
            Vitamin.inst.ui.Open<MainView>();
            
            //取得界面引用
            MainView mainView = Vitamin.inst.ui.Get<MainView>();
        }
    }
}