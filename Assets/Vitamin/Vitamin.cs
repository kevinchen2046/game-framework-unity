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
        System.Timers.Timer t = new System.Timers.Timer(time);//ʵ����Timer�࣬���ü��ʱ��Ϊ10000���룻
        t.Elapsed += new System.Timers.ElapsedEventHandler(method);//����ʱ���ʱ��ִ���¼���
        t.AutoReset = false;//������ִ��һ�Σ�false������һֱִ��(true)��
        t.Enabled = true;//�Ƿ�ִ��System.Timers.Timer.Elapsed�¼���
    }

    static public void Loop(int time, Action<object, System.Timers.ElapsedEventArgs> method)
    {
        System.Timers.Timer t = new System.Timers.Timer(time);//ʵ����Timer�࣬���ü��ʱ��Ϊ10000���룻
        t.Elapsed += new System.Timers.ElapsedEventHandler(method);//����ʱ���ʱ��ִ���¼���
        t.AutoReset = true;//������ִ��һ�Σ�false������һֱִ��(true)��
        t.Enabled = true;//�Ƿ�ִ��System.Timers.Timer.Elapsed�¼���
    }
}

