using System;
using UnityEngine;
using FairyGUI;
namespace vitamin
{
    public class Vitamin : MonoBehaviour
    {
        public static Vitamin inst { get { return FindObjectOfType<Vitamin>(); } }

        [Tooltip("UI基本设置")]
        public UIContentScaler scaler;

        [Header("上下文设置")]
        [Tooltip("继承自Context的UI入口")]
        public Context ui;
        [Tooltip("继承自Context的玩法入口")]
        public Context game;


        private Mananger __manager;
        public Mananger manager { get { return __manager; } }
       
        internal EventEmitter __center_emitter;
        void Awake()
        {
            Injector.initialize();

            __center_emitter = new EventEmitter();
            if (ui != null) ui.__emitter = __center_emitter;
            else Logger.Warn("UI Entry Not Found!");
            if (game != null) game.__emitter = __center_emitter;
            else Logger.Warn("Game Entry Not Found!");
            if(scaler==null)
            {
                Logger.Warn("UIContentScalar Not Found!");
            }
            __manager = ScriptableObject.CreateInstance<Mananger>();
            __manager.initialize();
            __manager.ui._emitter = __center_emitter;
           
            if (game != null)
            {
                game.__manager = __manager;
                game.initialize();
            }
            if (ui != null)
            {
                ui.__manager = __manager;
                ui.initialize();
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnGUI()
        {
            __manager.ui.Resize();
        }


        protected void onUIEvent<T>(string type, EventHandler<T> handler) where T : vitamin.Event
        {
            __center_emitter.on<T>(type, handler);
        }

        
        protected void emitEvent<T>(string type, params object[] data) where T : vitamin.Event
        {
            __center_emitter.emit<T>(type, data);
        }
    }
}
