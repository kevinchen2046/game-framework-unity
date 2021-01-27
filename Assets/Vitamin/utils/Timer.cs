using System;

namespace vitamin.utils
{
    public class Timer
    {

        static public void Delay(int time, Action<object, System.Timers.ElapsedEventArgs> method)
        {
            System.Timers.Timer t = new System.Timers.Timer(time);//实例化Timer类，设置间隔时间为10000毫秒；
            t.Elapsed += new System.Timers.ElapsedEventHandler(method);//到达时间的时候执行事件；
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        static public void Loop(int time, Action<object, System.Timers.ElapsedEventArgs> method)
        {
            System.Timers.Timer t = new System.Timers.Timer(time);//实例化Timer类，设置间隔时间为10000毫秒；
            t.Elapsed += new System.Timers.ElapsedEventHandler(method);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

    }
}
