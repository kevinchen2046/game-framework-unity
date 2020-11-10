using System.Collections;
using UnityEngine;
namespace vitamin
{
    public class Vitamin : MonoBehaviour
    {
        private string DefaultUIPackName;
        public void LoadUI(string path)
        {
            FairyGUI.UIPackage.AddPackage(path);
            DefaultUIPackName = path.LastIndexOf("/") >= 0 ? path.Substring(path.LastIndexOf("/"), path.Length) : path;
        }

        public void OpenUI(string uiname, string packname = null)
        {
            FairyGUI.GComponent view = FairyGUI.UIPackage.CreateObject(packname != null ? packname : DefaultUIPackName, uiname).asCom;
            FairyGUI.GRoot.inst.AddChild(view);
        }

        void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static Vitamin inst()
        {
            return GameObject.FindObjectOfType<Vitamin>();
        }
    }
}
