using vitamin;
using game.view;
using UnityEngine;

namespace game { 
    public class UI : Context
    {
        [Tooltip("多个资源请用,号隔开")]
        public string respack= "game";

        MainView mainView;
        override internal void initialize()
        {
            Vitamin.inst.manager.ui.Load(respack);
            Vitamin.inst.manager.ui.Register<MainView>(vitamin.UIType.FIX, "MainView");
     
            Vitamin.inst.manager.ui.Open<MainView>();
            mainView = Vitamin.inst.manager.ui.Get<MainView>();
        }
    }
}