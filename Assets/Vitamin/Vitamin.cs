using System.Collections;
using UnityEngine;
namespace vitamin
{
    public class Vitamin : MonoBehaviour
    {
        public UIManager ui;
        public FairyGUI.UIContentScaler scale;
    
        void Awake()
        {
            ui = new UIManager();
            scale = GameObject.FindObjectOfType<FairyGUI.UIContentScaler>();
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
                return GameObject.FindObjectOfType<Vitamin>();
            }
        }
    }
}
