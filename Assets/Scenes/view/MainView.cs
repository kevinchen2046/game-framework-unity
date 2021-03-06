using UnityEngine;
using vitamin;
using game.model;
namespace game.view
{
    public class MainView : ViewFairy
    {
        [Model]
        public ModelUser user;

        public FairyGUI.GComponent info;
        public FairyGUI.GButton btnTool;
        public FairyGUI.GButton btnSetting;
        public FairyGUI.GButton btnShare;

        public MainView(string uiname, string packname, vitamin.UIType uitype) : base(uiname, packname, uitype) { }

        override public void enter()
        {
            btnTool.onClick.Add(() =>
            {
                vitamin.Logger.Log(this.user.name);
                emitEvent<ButtonEvent>(ButtonEvent.TOOL, "Hello!");
            });
            btnSetting.onClick.Add(() =>
            {
                execCommand("user.rename", "KEVIN.CHEN");
                emitEvent<vitamin.Event>("SETTING_CLICK", "Hello!");
            });
            btnShare.onClick.Add(() =>
            {
                emitEvent<vitamin.Event>("SHARE_CLICK", "Hello!");

            });

            info.GetChild("btnClose").onClick.Add(() =>
            {
                vitamin.Logger.Debug(info.y, this.height);
                if (info.y < this.height - 200)
                {
                    Tween.Get(info).Prop(TweenProp.Y).To(this.height - 100).Ease(EaseType.CircInOut).Start(0.5f);
                }
                else
                {
                    Tween.Get(info).Prop(TweenProp.Y).To(this.height - info.height + 10).Ease(EaseType.CubicOut).Start(0.8f);
                }
            });
        }
        override internal void Resize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }
        private void onupdate(Vector3 v3)
        {
            if (v3 != null)
            {
                Debug.Log(v3.x.ToString());
            }
        }
    }

    public class ButtonEvent : vitamin.Event
    {
        public static string TOOL = "TOOL";
        public ButtonEvent(string type, params object[] args) : base(type, args) { }
    }
}