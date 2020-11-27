using UnityEngine;
using vitamin;
public class MainView : vitamin.ViewFairy
{
    [vitamin.Model]
    public ModelUser user;
    
    public FairyGUI.GComponent info;
    public FairyGUI.GButton btnSetting;
    public FairyGUI.GButton btnShare;
    public MainView(string uiname, string packname, vitamin.UIType uitype):base(uiname,packname,uitype){}
    
    override public void enter(){
        //Debug.Log(info);
        btnSetting.onClick.Add(() => {
            vitamin.Logger.Log("BtnSetting Click!!!");
        });
        btnShare.onClick.Add(() => {
            emitEvent<vitamin.Event>("DATA", "Hello!");
        });
        //Debug.Log(user);
        info.GetChild("btnClose").onClick.Add(() => {
            vitamin.Logger.Debug(info.y, this.height);
            Tween.Get(info).Prop(TweenProp.Y).To(this.height - 100).Ease(EaseType.CircInOut).Start(0.8f);
        });
        this.execCommand("user.rename","KevinChen");
    }

    private void onupdate(Vector3 v3)
    {
        if (v3 != null)
        {
            Debug.Log(v3.x.ToString());
        }
    }
}