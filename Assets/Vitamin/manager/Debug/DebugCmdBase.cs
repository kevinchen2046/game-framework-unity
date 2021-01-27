using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vitamin
{
    public abstract class DebugCmdBase
    {
        string __name;
        public string name { get { return __name; } }
        string __note;
        public string note { get { return __note; } }
        public DebugCmdBase(string name ,string note)
        {
            __name = name;
            __note = note;
        }

        abstract public void exec(string[] args, Action<string> complete);
    }
}
