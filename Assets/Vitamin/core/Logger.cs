using System;
namespace vitamin
{
    enum LoggerType
    {
        LOG, INFO, WARN, DEBUG, ERROR
    }
    class Logger
    {
        static public void log(params object[] args)
        {
            Logger.to(LoggerType.LOG, args);
        }

        static public void info(params object[] args)
        {
            Logger.to(LoggerType.INFO, args);
        }

        static public void warn(params object[] args)
        {
            Logger.to(LoggerType.WARN, args);
        }

        static public void debug(params object[] args)
        {
            Logger.to(LoggerType.DEBUG, args);
        }

        static public void error(params object[] args)
        {
            Logger.to(LoggerType.ERROR, args);
        }

        static  void to(LoggerType type, params object[] args)
        {
            if (!Config.log) return;
            string color = "";
            switch (type)
            {
                case LoggerType.LOG: color = "#0"; break;
                case LoggerType.INFO: color = "#00FF00"; break;
                case LoggerType.WARN: color = "#FFFF00"; break;
                case LoggerType.DEBUG: color = "#0000FF"; break;
                case LoggerType.ERROR: color = "#FF0000"; break;
            }
            string tag = nameof(type);
            Logger.display(tag,color,args);
        }
        static public void display(string tag,string color, params object[] args)
        {
            string content = tag + " ";
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
            // string formatstring="";
            // for(int i=0;i<args.Length;i++){
            //     formatstring+="{"+i+"}";
            // }
            string formatstring = "<b><color=" + color + ">{0}</color></b>";
            UnityEngine.Debug.Log(string.Format(formatstring, content));
            // switch(args.Length){
            //     case 1:UnityEngine.Debug.Log(string.Format(formatstring,args[0]));break;
            //     case 2:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1]));break;
            //     case 3:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1],args[2]));break;
            //     case 4:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1],args[2],args[3]));break;
            //     case 5:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1],args[2],args[3],args[4]));break;
            //     case 6:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1],args[2],args[3],args[4],args[5]));break;
            //     case 7:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1],args[2],args[3],args[4],args[5],args[6]));break;
            //     case 8:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1],args[2],args[3],args[4],args[5],args[6],args[7]));break;
            //     case 9:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1],args[2],args[3],args[4],args[5],args[6],args[7],args[8]));break;
            //     case 10:UnityEngine.Debug.Log(string.Format(formatstring,args[0],args[1],args[2],args[3],args[4],args[5],args[6],args[7],args[8],args[9]));break;
            // }

        }
    }
}