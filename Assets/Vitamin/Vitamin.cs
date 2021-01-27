using System;
using UnityEngine;
using FairyGUI;
using System.Collections;

namespace vitamin
{
    public class Vitamin : MonoBehaviour
    {
        public static Vitamin inst { get { return FindObjectOfType<Vitamin>(); } }

        [Header("基本设置")]
        [CustomInspectorAttribute("UI显示设置")]
        public UIContentScaler uiScaler;

        [Header("上下文")]
        [Tooltip("继承自vitamin.Context的UI上下文")]
        [CustomInspectorAttribute("界面上下文")]
        public Context uiContext;

        [Tooltip("继承自vitamin.Context的玩法上下文")]
        [CustomInspectorAttribute("玩法上下文")]
        public Context gameContext;

        [Header("FPS")]
        [CustomInspectorAttribute("显示FPS监测")]
        public bool fpsEnabled=false;

        [CustomInspectorAttribute("显示内存监测")]
        public bool memoryEnabled = false;

        [CustomInspectorAttribute("显示设备信息")]
        public bool deviceEnabled = false;

        [Header("调试")]
        [CustomInspectorAttribute("面板呼出热键")]
        public KeyCode debugHotKey=KeyCode.F7;

        UIManager __ui;
        AudioManager __sound;
        BoardManager __bord;
        GuideManager __guide;
        ResManager __res;
        DebugManager __debug;
        public UIManager ui { get { return __ui; } }
        public AudioManager sound { get { return __sound; } }
        public BoardManager bord { get { return __bord; } }
        public GuideManager guide { get { return __guide; } }
        public ResManager res { get { return __res; } }
        public DebugManager debug { get { return __debug; } }
        internal EventEmitter __center_emitter;

        void Awake()
        {
            Injector.initialize(typeof(DebugCmdBase));

            __center_emitter = new EventEmitter();
            if (uiContext != null) uiContext.__emitter = __center_emitter;
            else Logger.Warn("UI Entry Not Found!");
            if (gameContext != null) gameContext.__emitter = __center_emitter;
            else Logger.Warn("Game Entry Not Found!");
            if (uiScaler == null) Logger.Warn("UIContentScalar Not Found!");

            __ui = gameObject.AddComponent<UIManager>();
            __sound = gameObject.AddComponent<AudioManager>(); 
            __bord = gameObject.AddComponent<BoardManager>();
            __guide = gameObject.AddComponent<GuideManager>();
            __res = gameObject.AddComponent<ResManager>();
            __debug = gameObject.AddComponent<DebugManager>();

            __ui.initialize();
            if (debugHotKey != KeyCode.None)
            {
                __debug.initialize(fpsEnabled, memoryEnabled, deviceEnabled, debugHotKey);
            }

            ui._emitter = __center_emitter;

            if (gameContext != null) gameContext.initialize();
            if (ui != null) uiContext.initialize();
            Logger.Info("💊💊💊 ---------> Vitamin Start <--------- 💊💊💊");
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
            ui.Resize();
        }


        protected void onUIEvent<T>(string type, EventHandler<T> handler) where T : vitamin.Event
        {
            __center_emitter.on<T>(type, handler);
        }


        protected void emitEvent<T>(string type, params object[] data) where T : vitamin.Event
        {
            __center_emitter.emit<T>(type, data);
        }

        public void callLater(Action method)
        {
            StartCoroutine(callLaterHandler(method));
        }

        IEnumerator callLaterHandler(Action method)
        {
            yield return new WaitForEndOfFrame();
            method.Invoke();
        }

        public void wait(float seconds, Action method)
        {
            StartCoroutine(waitHandler(seconds, method));
        }

        IEnumerator waitHandler(float seconds, Action method)
        {
            yield return new WaitForSeconds(seconds);
            method.Invoke();
        }
    }
}
