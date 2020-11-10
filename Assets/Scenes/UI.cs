using System.Collections;
using UnityEngine;

public class UI : MonoBehaviour
{
    void Awake(){
        vitamin.Vitamin.inst().LoadUI("game");
        vitamin.Vitamin.inst().OpenUI("MainView");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
