﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vitamin{

    public delegate void EventHandler(params object[] args);

    public class EventEmitter
    {

        private Dictionary<string, List<EventHandler>> _events;

        /// <summary>
        /// The EventEmitter object to subscribe to events with
        /// </summary>
        public EventEmitter()
        {
            this._events = new Dictionary<string, List<EventHandler>>();
        }

        /// <summary>
        /// Whenever eventName is emitted, the methods attached to this event will be called
        /// </summary>
        /// <param name="eventName">Event name to subscribe to</param>
        /// <param name="method">Method to add to the event</param>
        public void on(string eventName, EventHandler method)
        {
            List<EventHandler> subscribedMethods;
            if (this._events.TryGetValue(eventName, out subscribedMethods))
            {
                subscribedMethods.Add(method);
            }
            else
            {
                this._events.Add(eventName, new List<EventHandler> { method });
            }
        }

        /// <summary>
        /// Emits the event and associated data
        /// 发出事件和相关数据
        /// </summary>
        /// <param name="eventName">Event name to be emitted</param>
        /// <param name="data">Data to call the attached methods with</param>
        public void emit(string eventName, params object[] data)
        {
            List<EventHandler> subscribedMethods;
            if (!this._events.TryGetValue(eventName, out subscribedMethods))
            {
                Logger.warn(string.Format("Event [{0}] does not exist in the emitter. Consider calling EventEmitter.On", eventName));
            }
            else
            {
                foreach (var f in subscribedMethods)
                {
                    f(data);
                }
            }
        }

        /// <summary>
        /// Removes [method] from the event
        /// </summary>
        /// <param name="eventName">Event name to remove function from</param>
        /// <param name="method">Method to remove from eventName</param>
        public void removeListener(string eventName, EventHandler method)
        {
            List<EventHandler> subscribedMethods;
            if (!this._events.TryGetValue(eventName, out subscribedMethods))
            {
               Logger.warn(string.Format("Event [{0}] does not exist to have listeners removed.", eventName));
            }
            else
            {
                var _event = subscribedMethods.Exists(e => e == method);
                if (_event == false)
                {
                   Logger.warn(string.Format("Func [{0}] does not exist to be removed.", method.Method));
                }
                else
                {
                    subscribedMethods.Remove(method);
                }                
            }
        }

        /// <summary>
        /// Removes all methods from the event [eventName]
        /// </summary>
        /// <param name="eventName">Event name to remove methods from</param>
        public void removeAllListeners(string eventName)
        {
            List<EventHandler> subscribedMethods;
            if (!this._events.TryGetValue(eventName, out subscribedMethods))
            {
               Logger.warn(string.Format("Event [{0}] does not exist to have methods removed.", eventName));
            }
            else
            {
                subscribedMethods.RemoveAll(x => x != null);
            }
        }

        /// <summary>
        /// Emits the event and runs all associated methods asynchronously
        /// 发出事件并异步运行所有关联的方法
        /// </summary>
        /// <param name="eventName">The event name to call methods for</param>
        /// <param name="data">The data to call all the methods with</param>
        public void emitAsync(string eventName, object data)
        {
            List<EventHandler> subscribedMethods;
            if (!this._events.TryGetValue(eventName, out subscribedMethods))
            {
               Logger.warn(string.Format("Event [{0}] does not exist in the emitter. Consider calling EventEmitter.On", eventName));
            }
            else
            {
                foreach (var f in subscribedMethods)
                {
                    Task.Run(() => f(data));
                }
            }
        }
    }
}
