using UnityEngine;

using vitamin;
public class Mananger : ScriptableObject
{
    public UIManager ui;
    public AudioManager audio;
    public BoardManager bord;
    public GuideManager guide;
    public ResManager res;
    public DebugManager debug;
    public void initialize()
    {
        ui = new UIManager();
        audio = new AudioManager();
        bord = new BoardManager();
        guide = new GuideManager();
        res = new ResManager();
        debug = new DebugManager();
    }
}