using System;
using System.Collections;
using UnityEngine;
using vitamin;
using FairyGUI;
public class Vitamin : MonoBehaviour
{
    private UIContentScaler _scaleconfig;

    private Mananger _manager;
    public Mananger manager { get { return this._manager; } }
    private Entry[] _entrys;

    void Awake()
    {
        Injector.initialize();
        _scaleconfig = gameObject.GetComponent<UIContentScaler>();

        EventEmitter uiEmitter = new EventEmitter();
        EventEmitter gameEmitter = new EventEmitter();
        _manager = ScriptableObject.CreateInstance<Mananger>();
        _manager.initialize();

        _manager.ui.initialize(uiEmitter, gameEmitter);
        // scale.designResolutionX=640;
        // scale.designResolutionY=1136;
        // scale.scaleMode=FairyGUI.UIContentScaler.ScaleMode.ScaleWithScreenSize;
        // scale.screenMatchMode=FairyGUI.UIContentScaler.ScreenMatchMode.MatchWidthOrHeight;
        _entrys = gameObject.GetComponents<Entry>();
        if (_entrys.Length>0)
        {
            foreach(Entry entry in _entrys)
            {
                entry.manager = _manager;
                entry.uiEmitter = uiEmitter;
                entry.gameEmitter = gameEmitter;
            }
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

    public static Vitamin inst
    {
        get{
            return FindObjectOfType<Vitamin>();
        }
    }
    
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

