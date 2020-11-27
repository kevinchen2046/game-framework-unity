// ----------------------------------------------------------------------------
// Example:
// EventEmitter emitter=new EventEmitter();

// class MyEvent : Event
// {
//     public static string ENTER = "ENTER";
//     public string name = "myname is.xxx";
//     public MyEvent(string type, params object[] data) : base(type, data)
//     {
//     }
// }

// emitter.on<Event>("ENTER", （Event e)=>{
//      Logger.log(e.ToString());
// });
// emitter.on<MyEvent>(MyEvent.ENTER, (MyEvent e)=>{
//      Logger.log(e.ToString());
// });

// emitter.emit<Event>("ENTER", "a", "bc", "hello!!");
// emitter.emit<MyEvent>(MyEvent.ENTER, 1, 2, 3);
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vitamin
{
    public class Event
    {
        public Event(string type, params object[] args)
        {
            this.type = type;
            this.somedata = args;
            if (this.somedata.Length > 0)
            {
                this.data = this.somedata[0];
            }
        }
        public string type;
        public object data;
        private object[] _somedata;
        public object[] somedata
        {
            set
            {
                _somedata = value;
                if (this._somedata.Length > 0)
                {
                    this.data = this._somedata[0];
                }
            }
            get
            {
                return this._somedata;
            }

        }

        public override string ToString()
        {
            string datastring = "";
            foreach (var value in this.somedata)
            {
                datastring += value.ToString() + ",";
            }
            datastring = datastring.Substring(0, datastring.Length - 1);

            return "[ " + this.GetType().ToString() + " ] { type: " + this.type + ", data: " + datastring + "}";
        }
    }

    public delegate void EventHandler<T>(T e) where T : Event;

    public class EventEmitter
    {
        private Event _event;
        private Dictionary<string, List<object>> _events;

        /// <summary>
        /// The EventEmitter object to subscribe to events with
        /// </summary>
        public EventEmitter()
        {
            this._events = new Dictionary<string, List<object>>();
            this._event = new Event("");
        }

        /// <summary>
        /// Whenever eventName is emitted, the methods attached to this event will be called
        /// </summary>
        /// <param name="eventName">Event name to subscribe to</param>
        /// <param name="handler">Method to add to the event</param>
        public void on<T>(string type, vitamin.EventHandler<T> handler) where T : Event
        {
            List<object> handlers;
            if (this._events.TryGetValue(type, out handlers))
            {
                handlers.Add(handler);
            }
            else
            {
                this._events.Add(type, new List<object> { handler });
            }
        }

        /// <summary>
        /// Emits the event and associated data
        /// 发出事件和相关数据
        /// </summary>
        /// <param name="eventName">Event name to be emitted</param>
        /// <param name="data">Data to call the attached methods with</param>
        public void emit(string type, params object[] data)
        {
            Type eventType = this._event.GetType();
            this._event.type = type;
            this._event.somedata = data;
            List<object> handlers;
            if (!this._events.TryGetValue(type, out handlers))
            {
                vitamin.Logger.Warn(string.Format("Event [{0}] does not exist in the emitter. Consider calling EventEmitter.On", type));
            }
            else
            {
                foreach (var handler in handlers)
                {
                    if (handler.GetType().GenericTypeArguments[0] == eventType)
                    {
                        vitamin.EventHandler<Event> eventHandler = (vitamin.EventHandler<Event>)handler;
                        eventHandler(this._event);
                    }
                }
            }
        }
        public void emit<T>(T e, params object[] data) where T : Event
        {
            Type eventType = typeof(T);
            List<object> handlers;
            if (!this._events.TryGetValue(e.type, out handlers))
            {
                Logger.Warn(string.Format("Event [{0}] does not exist in the emitter. Consider calling EventEmitter.On", e.type));
            }
            else
            {
                e.somedata = data;
                foreach (var handler in handlers)
                {
                    if (handler.GetType().GenericTypeArguments[0] == eventType)
                    {
                        vitamin.EventHandler<T> eventHandler = (vitamin.EventHandler<T>)handler;
                        eventHandler(e);
                    }
                }
            }
        }

        public void emit<T>(string type, params object[] data) where T : Event
        {
            Type eventType = typeof(T);
            T e = Activator.CreateInstance(eventType, type) as T;
            e.somedata = data;
            List<object> handlers;
            if (!this._events.TryGetValue(e.type, out handlers))
            {
                Logger.Warn(string.Format("Event [{0}] does not exist in the emitter. Consider calling EventEmitter.On", e.type));
            }
            else
            {
                e.somedata = data;
                foreach (var handler in handlers)
                {
                    if (handler.GetType().GenericTypeArguments[0] == eventType)
                    {
                        vitamin.EventHandler<T> eventHandler = (vitamin.EventHandler<T>)handler;
                        eventHandler(e);
                    }
                }
            }
        }

        /// <summary>
        /// Removes [method] from the event
        /// </summary>
        /// <param name="eventName">Event name to remove function from</param>
        /// <param name="method">Method to remove from eventName</param>
        public void removeListener<T>(string eventName, vitamin.EventHandler<T> method) where T : Event
        {
            List<object> handlers;
            if (!this._events.TryGetValue(eventName, out handlers))
            {
                Logger.Warn(string.Format("Event [{0}] does not exist to have listeners removed.", eventName));
            }
            else
            {
                var _event = handlers.Exists(e => (vitamin.EventHandler<T>)e == method);
                if (_event == false)
                {
                    Logger.Warn(string.Format("Func [{0}] does not exist to be removed.", method.Method));
                }
                else
                {
                    handlers.Remove(method);
                }
            }
        }

        /// <summary>
        /// Removes all methods from the event [eventName]
        /// </summary>
        /// <param name="type">Event name to remove methods from</param>
        public void removeAllListeners(string type)
        {
            List<object> handlers;
            if (!this._events.TryGetValue(type, out handlers))
            {
                Logger.Warn(string.Format("Event [{0}] does not exist to have methods removed.", type));
            }
            else
            {
                handlers.RemoveAll(x => x != null);
            }
        }

        /// <summary>
        /// Emits the event and runs all associated methods asynchronously
        /// 发出事件并异步运行所有关联的方法
        /// </summary>
        /// <param name="type">The event name to call methods for</param>
        /// <param name="data">The data to call all the methods with</param>
        public void emitAsync(string type, params object[] data)
        {
            this._event.type = type;
            this._event.somedata = data;
            List<object> handlers;
            if (!this._events.TryGetValue(type, out handlers))
            {
                Logger.Warn(string.Format("Event [{0}] does not exist in the emitter. Consider calling EventEmitter.On", type));
            }
            else
            {
                foreach (var handler in handlers)
                {
                    vitamin.EventHandler<Event> eventHandler = (vitamin.EventHandler<Event>)handler;
                    Task.Run(() => eventHandler(this._event));
                }
            }
        }

        public void emitAsync<T>(T e, params object[] data) where T : Event
        {
            List<object> handlers;
            if (!this._events.TryGetValue(e.type, out handlers))
            {
                Logger.Warn(string.Format("Event [{0}] does not exist in the emitter. Consider calling EventEmitter.On", e.type));
            }
            else
            {
                foreach (var handler in handlers)
                {
                    vitamin.EventHandler<T> eventHandler = (vitamin.EventHandler<T>)handler;
                    Task.Run(() => eventHandler(e));
                }
            }
        }
    }
}
