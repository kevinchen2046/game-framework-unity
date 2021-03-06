using System;
using System.Reflection;

namespace vitamin
{
    public interface IModel {
        void initialize();
        void reset();
    }
    public class ModelBase : EventEmitter,IModel
    {
        public ModelBase()
        {

        }

        public virtual void initialize() { }
        public virtual void reset() { }
    }

    public abstract class ViewBase
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

    public class CommandBase
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