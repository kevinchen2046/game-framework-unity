using System;
using System.Reflection;

namespace vitamin
{
    class ModelBase : EventEmitter
    {
        public ModelBase()
        {

        }

        public virtual void initialize() { }
        public virtual void reset() { }
    }

    class ViewBase : EventEmitter
    {
        public ViewBase()
        {

        }

        public virtual void enter()
        {
            
            //Vitamin.addCommandEnd(this);
        }

        public virtual void exit()
        {
            //Vitamin.removeCommandEnd(this);
        }

        protected void execCommand(string cmdRoute, params object[] args)
        {
            Injector.execCommand(cmdRoute,args);
        }
    }

    class CommandBase : EventEmitter
    {
        public Net net;
        public CommandBase()
        {

        }

        public virtual void exec(params object[] args)
        {

        }
    }
}