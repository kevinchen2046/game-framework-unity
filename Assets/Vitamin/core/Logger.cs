using System;
namespace vitamin
{
    class Logger
    {
        static public void log(params object[] args)
        {
            Logger.to("[LOG]", "#0",args);
        }

        static public void info(params object[] args)
        {
            Logger.to("[INFO]", "#00FF00", args);
        }

        static public void warn(params object[] args)
        {
            Logger.to("[WARN]", "#FFFF00", args);
        }

        static public void debug(params object[] args)
        {
            Logger.to("[DEBUG]", "#0000FF", args);
        }

        static public void error(params object[] args)
        {
            Logger.to("[ERROR]", "#FF0000", args);
        }

        static public void to(String tag,string color, params object[] args)
        {
            if (!Config.log) return;
            String content = tag + " ";
            foreach (object arg in args)
            {
                content += arg.ToString() + " ";
            }
            UnityEngine.Debug.Log(string.Format("<color=" + color + ">{0}</color>", content));
        }
    }
}