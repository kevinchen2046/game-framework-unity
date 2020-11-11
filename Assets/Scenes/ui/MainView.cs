using UnityEngine;
public class MainView : vitamin.ViewFairy
{
    [vitamin.Model]
    public ModelUser user;
    public FairyGUI.GComponent info;
    public FairyGUI.GButton btnSetting;
    public MainView(string uiname, string packname, vitamin.UIType uitype):base(uiname,packname,uitype){}
    
    override public void enter(){
        Debug.Log(info);
        btnSetting.onClick.Add(()=>{
            vitamin.Logger.log("BtnSetting Click!!!");
        });
        vitamin.Logger.log(user.name);

        this.execCommand("user.rename","KevinChen");
    }
}