# game-framework-unity

unity+fairygui 的游戏框架

## Vitamin 核心功能实现

### 开始之前需要挂载的脚本

- FairyGUI/Context.cs 用于启动应用
- Vitamin/UIContentScaler.cs 用于设置UI基本属性

除此以外场景可以任意编辑,UI不会与其产生任何冲突

### 基本使用方法


```c#
    //加载资源
    vitamin.Context.inst.ui.Load("game");
    //注册界面
    vitamin.Context.inst.ui.Register(vitamin.UIType.FIX, typeof(MainView), "MainView");
    //打开界面
    vitamin.Context.inst.ui.Open<MainView>();
    //获取界面引用
    vitamin.Context.inst.ui.Get<MainView>();
```

### 命令实现 Command
```c#
// 这是一个 Commnad的实例
// 这里需要定义一个Command的路由 用于触发Command
[vitamin.CmdRoute("user.rename")]
public class CmdRename : vitamin.CommandBase
{
    //在Command可以注入任意的Model
    [vitamin.Model]
    public ModelUser user;
    //Command被执行
    public override void exec(params object[] args)
    {
        user.name=args[0].ToString();
        vitamin.Logger.debug("CmdRename:" + this.user.name);
    }
}
```

### 数据模块实现 Model
```c#
using UnityEngine;
public class ModelUser : vitamin.ModelBase
{
    public string name = "我是ModelUser";
    public ModelUser(){
        
    }

    override public void initialize(){
        vitamin.Logger.log("ModelUser initialize!");
    }
}
```

### 界面实现 View
```c#
using UnityEngine;
//需要继承 FairyGUI 的视图适配基类 ViewFairy
public class MainView : vitamin.ViewFairy
{
    //注入Model
    [vitamin.Model]
    public ModelUser user;
    
    //FairyGui编辑器内定义的组件会自动注入
    public FairyGUI.GComponent info;
    public FairyGUI.GButton btnSetting;

    public MainView(string uiname, string packname, vitamin.UIType uitype):base(uiname,packname,uitype){}
    
    override public void enter(){

        //可直接使用的控件
        Debug.Log(info);
        btnSetting.onClick.Add(()=>{
            vitamin.Logger.log("BtnSetting Click!!!");
        });
        //可以使用注入的Model
        Debug.Log(user);

        //执行Command
        this.execCommand("user.rename","KevinChen");
    }
}
```