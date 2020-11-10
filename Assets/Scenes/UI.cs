using System.Collections;
using UnityEngine;

public class UI : MonoBehaviour
{
    MainView mainView;
    void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        vitamin.Vitamin.inst.ui.Load("game");
        vitamin.Vitamin.inst.ui.Register(vitamin.UIType.FIX,typeof(MainView),"MainView");
        mainView = vitamin.Vitamin.inst.ui.Open<MainView>();
        
        FairyGUI.Stage.inst.onStageResized.Add(Resize);
        Resize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Resize()
    {
        mainView.width = FairyGUI.GRoot.inst.width;
        mainView.height = FairyGUI.GRoot.inst.height;
    }
}
