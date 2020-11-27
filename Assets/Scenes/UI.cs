using System.Collections;
using UnityEngine;

public class UI : Entry
{
    MainView mainView;
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Vitamin.inst.manager.ui.Load("game");
        Vitamin.inst.manager.ui.Register<MainView>(vitamin.UIType.FIX, "MainView");
        Vitamin.inst.manager.ui.Open<MainView>();
        mainView = Vitamin.inst.manager.ui.Get<MainView>();
        //FairyGUI.Stage.inst.onStageResized.Add(Resize);
        OnGUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (this.mainView != null)
        {
            mainView.width = FairyGUI.GRoot.inst.width;
            mainView.height = FairyGUI.GRoot.inst.height;
        }
    }
}
