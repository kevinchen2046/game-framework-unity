using UnityEngine;

namespace vitamin
{
    public class Context : MonoBehaviour
    {

        internal EventEmitter __emitter;
        public Context()
        {

        }

        virtual internal void initialize()
        {

        }

        virtual internal void reset()
        {

        }

        protected void onEvent<T>(string type, EventHandler<T> handler) where T : vitamin.Event
        {
            __emitter.on<T>(type, handler);
        }
        protected void emitEvent<T>(string type, params object[] data) where T : vitamin.Event
        {
            __emitter.emit<T>(type, data);
        }
    }
}