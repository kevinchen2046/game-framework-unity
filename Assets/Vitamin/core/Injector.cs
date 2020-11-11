using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vitamin
{
    public class Config
    {
        static public bool log = true;
    }

    public class Injector
    {
        static Dictionary<Type, ViewBase> __views;
        static Dictionary<Type, IModel>  __modles;
        static Dictionary<string, CommandBase> __cmds;
        static Dictionary<Type, object> __instances;

        static public void initialize()
        {
            Injector.__views = new Dictionary<Type, ViewBase>();
            Injector.__modles = new Dictionary<Type, IModel>();
            Injector.__cmds = new Dictionary<string, CommandBase>();
            Injector.__instances = new Dictionary<Type, object>();

            var types = Assembly.GetCallingAssembly().GetTypes();
            var modelBaseType = typeof(ModelBase);
            var viewBaseType = typeof(ViewBase);
            var cmdBaseType = typeof(CommandBase);

            var net = new Net();
            foreach (var type in types)
            {
                //获取基类
                var baseType = type.BaseType;
                while (true)
                {   
                    if(baseType==null) break;
                    if(baseType==modelBaseType) break;
                    if(baseType==viewBaseType) break;
                    if(baseType==cmdBaseType) break;
                    if(baseType.BaseType==null) break;
                    baseType=baseType.BaseType;
                }

                if (baseType == modelBaseType)
                {
                    IModel model = Activator.CreateInstance(type) as IModel;
                    Injector.__modles.Add(type, model);
                }
                else if (baseType == viewBaseType)
                {
                    Injector.__views.Add(type, null);
                }
                else if (baseType == cmdBaseType)
                {
                    CmdRoute des = (CmdRoute)type.GetCustomAttribute(typeof(CmdRoute));
                    if (des == null)
                    {
                        Logger.error(baseType.ToString() + "没有添加描述信息!");
                    }
                    else
                    {
                        CommandBase cmd = Activator.CreateInstance(type) as CommandBase;
                        cmd.net = net;
                        Injector.__cmds.Add(des.routId, cmd);
                    }
                }
            }

            while (true)
            {
                var allInject = true;
                foreach (var model in Injector.__modles)
                {
                    bool result = Injector.injectModel(model.Value, model.Value.GetType());
                    if (!result) allInject = false;
                }
                if (allInject)
                {
                    foreach (var model in Injector.__modles)
                    {
                        Injector.injectInstance(model.Value, model.Value.GetType());
                        model.Value.initialize();
                    }
                    break;
                }
            }

            foreach (var cmd in Injector.__cmds)
            {
                Injector.injectInstance(cmd.Value, cmd.Value.GetType());
                bool result = Injector.injectModel(cmd.Value, cmd.Value.GetType());
            }
            Logger.info("🎇✨🎉✨🛠💊 - Vitamin Start - 💊🛠✨🎉✨🎇");
        }

        /// <summary>
        /// 注入单例
        /// 仅供内部调用，如果你希望框架外的类有依赖注入，请使用createObject实例化该类
        /// </summary>
        static private bool injectInstance(object target, Type type)
        {
            bool result = true;
            var instanceType = typeof(Instance);
            FieldInfo[] filedInfos = type.GetFields();
            foreach (FieldInfo info in filedInfos)
            {
                if (Attribute.IsDefined(info, instanceType))
                {
                    info.SetValue(target, Injector.getInstance(info.FieldType));
                }
            }
            return result;
        }

        /// <summary>
        /// 获取单例
        /// 通过此接口获取的单例会有相关的依赖注入
        /// </summary>
        static public object getInstance(Type type)
        {
            if (Injector.__instances[type] == null)
            {
                object instance = Activator.CreateInstance(type);
                Injector.injectModel(instance, instance.GetType());
                Injector.__instances[type] = instance;
            }
            return Injector.__instances[type];
        }

        /// <summary>
        /// 创建实例
        /// 此方法适用于框架外的类有依赖注入的需求的情况，请使用该方法实例化该类
        /// </summary>
        static public object createObject(Type type)
        {
            object obj = Activator.CreateInstance(type);
            Injector.injectModel(obj, type);
            Injector.injectInstance(obj, type);
            return obj;
        }

        /// <summary>
        /// 注入Model
        /// 通过框架接口获取的组件才会有相关的依赖注入
        /// </summary>
        static private bool injectModel(object target, Type type)
        {
            bool result = true;
            var modelType = typeof(Model);
            FieldInfo[] filedInfos = type.GetFields();
            foreach (FieldInfo info in filedInfos)
            {
                if (Attribute.IsDefined(info, modelType))
                {
                    if (!Injector.__modles.ContainsKey(info.FieldType))
                    {
                        result = false;
                        continue;
                    }
                    info.SetValue(target, Injector.__modles[info.FieldType]);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取组件
        /// 通过框架接口获取的组件才会有相关的依赖注入
        /// </summary>
        static public ViewBase createView(Type viewType,params object[] args)
        {
            if (Injector.__views.ContainsKey(viewType))
            {
                if (Injector.__views[viewType] == null)
                {
                    ViewBase view = (ViewBase)Activator.CreateInstance(viewType,args);
                    Injector.injectModel(view, viewType);
                    Injector.__views[viewType] = view;
                }
            }
            return Injector.__views[viewType];
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        static public void execCommand(string cmdRoute, params object[] args)
        {

            if (Injector.__cmds.ContainsKey(cmdRoute))
            {
                CommandBase cmd = Injector.__cmds[cmdRoute];
                if (cmd != null)
                {
                    cmd.exec(args);
                }
                else
                {
                    Logger.error("无法执行命令:" + cmdRoute);
                }
            }
        }

        /// <summary>
        /// 输出对象反射信息
        /// </summary>
        static public void reflex(object instance)
        {
            Logger.log(instance.ToString());
            Injector.logFileds(instance.GetType());
            Injector.logPropertys(instance.GetType());
            Injector.logMethods(instance.GetType());
        }

        /// <summary>
        /// 输出类型反射信息
        /// </summary>
        static public void reflex(Type type)
        {
            Logger.display("[REFLEX]", "#00FFFF", "-----------[" + type.ToString() + "]------------");
            Injector.logFileds(type);
            Injector.logPropertys(type);
            Injector.logMethods(type);
            Logger.display("[REFLEX]", "#00FFFF", "------------------------------------------");
        }

        static private void logFileds(Type classType)
        {
            FieldInfo[] filedInfos = classType.GetFields();
            Logger.display("[REFLEX]", "#00FFFF", "    " + "字段[" + filedInfos.Length + "]:");
            foreach (FieldInfo info in filedInfos)
            {
                Logger.display("[REFLEX]", "#FF00FF", "            - " + info.Name);
            }
        }
        static private void logMethods(Type classType)
        {
            MethodInfo[] methods = classType.GetMethods();
            Logger.display("[REFLEX]", "#00FFFF", "    " + "方法[" + methods.Length + "]:");
            foreach (MethodInfo info in methods)
            {
                Logger.display("[REFLEX]", "#FF00FF", "            - " + info.Name);
            }
        }
        static private void logPropertys(Type classType)
        {
            PropertyInfo[] properties = classType.GetProperties();
            Logger.display("[REFLEX]", "#00FFFF", "    " + "属性[" + properties.Length + "]:");
            foreach (PropertyInfo info in properties)
            {
                Logger.display("[REFLEX]", "#FF00FF","            - " + info.Name);
            }
        }

        static public void delay(int time, Action<object, System.Timers.ElapsedEventArgs> method)
        {
            System.Timers.Timer t = new System.Timers.Timer(time);//实例化Timer类，设置间隔时间为10000毫秒；
            t.Elapsed += new System.Timers.ElapsedEventHandler(method);//到达时间的时候执行事件；
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        static public void loop(int time, Action<object, System.Timers.ElapsedEventArgs> method)
        {
            System.Timers.Timer t = new System.Timers.Timer(time);//实例化Timer类，设置间隔时间为10000毫秒；
            t.Elapsed += new System.Timers.ElapsedEventHandler(method);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }
    }

    /**
     * 单例装饰器
     * @param clazz 需要单例化的class对象
     */
    [AttributeUsage(AttributeTargets.Field)]
    public class Instance : Attribute
    {
        public Instance() { }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CmdRoute : Attribute
    {
        public string routId;
        public CmdRoute(string routeId)
        {
            this.routId = routeId;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class Model : Attribute
    {
        public Model() { }
    }
}