namespace vitamin
{
    public class ViewFairy : ViewBase
    {
        protected FairyGUI.GComponent skin;
        public UIType uitype;
        private string uiname;
        private string packname;
        internal EventEmitter _emitter;
        public ViewFairy(string uiname, string packname,UIType uitype)
        {
            this.uiname=uiname;
            this.packname=packname;
            this.uitype=uitype;
        }
        public float x{
            get{return this.skin.x;}
            set{this.skin.x=value;}
        }
        public float y{
            get{return this.skin.y;}
            set{this.skin.y=value;}
        }
        public float width{
            get{return this.skin.width;}
            set{this.skin.width=value;}
        }
        public float height{
            get{return this.skin.height;}
            set{this.skin.height=value;}
        }
        void Create(){
            skin = FairyGUI.UIPackage.CreateObject(packname, uiname).asCom;
            foreach(FairyGUI.GObject gobject in skin._children){
                Util.SetProperty(this,gobject.name,gobject);
            }
        }

        virtual internal void Resize(float width, float height)
        {

        }

        public void Add(FairyGUI.GComponent container){
            if(this.skin==null){
                this.Create();
            }
            container.AddChild(this.skin);
        }

        public void Remove(){
            if(this.skin!=null){
                if(this.skin.parent!=null){
                    this.skin.parent.RemoveChild(this.skin);
                }
            }
        }

        protected void onEvent<T>(string type, EventHandler<T> handler) where T : vitamin.Event
        {
            _emitter.on<T>(type, handler);
        }

        protected void emitEvent<T>(string type, params object[] data) where T : vitamin.Event
        {
            _emitter.emit<T>(type, data);
        }
    }
}