using System.Collections;
using UnityEngine;
namespace vitamin
{
    public class Context : MonoBehaviour
    {
        public UIManager ui;
        public FairyGUI.UIContentScaler scale;
    
        void Awake()
        {
            Injector.initialize();
            gameObject.GetComponent<FairyGUI.UIContentScaler>();
            scale = gameObject.GetComponent<FairyGUI.UIContentScaler>();
            // scale.designResolutionX=640;
            // scale.designResolutionY=1136;
            // scale.scaleMode=FairyGUI.UIContentScaler.ScaleMode.ScaleWithScreenSize;
            // scale.screenMatchMode=FairyGUI.UIContentScaler.ScreenMatchMode.MatchWidthOrHeight;
            gameObject.AddComponent<UI>();
            gameObject.AddComponent<Game>();
            ui = new UIManager();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static Context inst
        {
            get{
                return GameObject.FindObjectOfType<Context>();
            }
        }
    }
}
