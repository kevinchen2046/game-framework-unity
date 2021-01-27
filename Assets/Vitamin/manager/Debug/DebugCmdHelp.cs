using System;
using System.Collections.Generic;

namespace vitamin
{
    public class DebugCmdHelp:DebugCmdBase
    {
        public DebugCmdHelp() : base("help","帮助"){}

        override public void exec(string[] args, Action<string> complete)
        {
            List<string> contents = new List<string>();
            foreach(var cmd in Vitamin.inst.debug.cmds)
            {
                contents.Add(string.Format("    ◆ {0}         {1}", cmd.name,cmd.note));
            }
            contents.Add("添加调试命令请继承`vitamin.DebugCmdBase`.\n示例参考`vitamin.DebugCmdHelp`");
            complete.Invoke(string.Join("\n", contents));
        }
    }
}
