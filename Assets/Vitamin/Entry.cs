using UnityEngine;
using System.Collections;
using vitamin;
public class Entry : MonoBehaviour
{
    private EventEmitter _uiEmitter;
    public EventEmitter uiEmitter { set { _uiEmitter = value; } }
    private EventEmitter _gameEmitter;
    public EventEmitter gameEmitter { set { _gameEmitter = value; } }
    protected Mananger _manager;
    public Mananger manager { set { _manager = value; } }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void onUIEvent<T>(string type, EventHandler<T> handler) where T : vitamin.Event
    {
        _uiEmitter.on<T>(type, handler);
    }

    protected void onGameEvent<T>(string type, EventHandler<T> handler) where T : vitamin.Event
    {
        _gameEmitter.on<T>(type, handler);
    }

    protected void emitUIEvent<T>(string type, params object[] data) where T : vitamin.Event
    {
        _uiEmitter.emit<T>(type, data);
    }

    protected void emitGameEvent<T>(string type, params object[] data) where T : vitamin.Event
    {
        _gameEmitter.emit<T>(type, data);
    }
}
