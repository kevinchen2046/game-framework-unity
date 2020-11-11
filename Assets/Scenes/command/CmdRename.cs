// 这是一个 Commnad的实例
// 这里需要定义一个Command的路由 用于触发Command
[CmdRoute("user.rename")]
public class CmdRename : CommandBase
{
    //在Command可以注入任意的Model
    [Model]
    public ModelUser user;
    //Command被执行
    public override void exec(params object[] args)
    {
        user.name=args[0].toString();
        vitamin.Logger.debug("CmdRename:" + this.user.name);
    }
}