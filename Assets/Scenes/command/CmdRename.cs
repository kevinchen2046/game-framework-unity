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