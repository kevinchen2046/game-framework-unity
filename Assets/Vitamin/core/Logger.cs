using System;
namespace vitamin
{
    enum LoggerType
    {
        LOG, INFO, WARN, DEBUG, ERROR, LIST
    }
    public class Logger
    {
        static public void List(object[] args)
        {
            Logger.to(LoggerType.LIST, ConsoleColor.White, vitamin.CollectionUtil.Join(args, ","));
        }
        static public void List(int[] args)
        {
            Logger.to(LoggerType.LIST, ConsoleColor.White, vitamin.CollectionUtil.Join(args, ","));
        }
        static public void List(string[] args)
        {
            Logger.to(LoggerType.LIST, ConsoleColor.White, vitamin.CollectionUtil.Join(args, ","));
        }
        static public void List(float[] args)
        {
            Logger.to(LoggerType.LIST, ConsoleColor.White, vitamin.CollectionUtil.Join(args, ","));
        }

        static public void Log(params object[] args)
        {
            Logger.to(LoggerType.LOG, args);
        }

        static public void Info(params object[] args)
        {
            Logger.to(LoggerType.INFO, args);
        }

        static public void Warn(params object[] args)
        {
            Logger.to(LoggerType.WARN, args);
        }

        static public void Debug(params object[] args)
        {
            Logger.to(LoggerType.DEBUG, args);
        }

        static public void Error(params object[] args)
        {
            Logger.to(LoggerType.ERROR, args);
        }

        static void to(LoggerType type, params object[] args)
        {
            if (!Config.log) return;
            string color = "";
            switch (type)
            {
                case LoggerType.LOG: color = "#33383c"; break;
                case LoggerType.INFO: color = "#296522"; break;
                case LoggerType.WARN: color = "#947e25"; break;
                case LoggerType.DEBUG: color = "#073763"; break;
                case LoggerType.ERROR: color = "#942525"; break;
            }
            string tag = "[" + Enum.GetName(typeof(LoggerType), type) + "]";
            Logger.display(tag, color, args);
        }
        static public void display(string tag, string color, params object[] args)
        {
            string content = "";
            foreach (object arg in args)
            {
                if (content == null)
                {
                    content += "Null ";
                    continue;
                }
                if (arg is string || arg is bool || arg is float || arg is int)
                {
                    content += arg.ToString() + " ";
                }
                else if (arg is Type)
                {
                    content += arg.ToString() + " ";
                }
                else
                {
                    content += arg.GetType().ToString() + " ";
                }
            }
            string formatstring = "<color=" + color + ">{0}</color><color=" + color + ">  {1}</color>";
            UnityEngine.Debug.Log(string.Format(formatstring, tag, content));
        }
    }
}