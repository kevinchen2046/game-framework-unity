namespace vitamin
{

    public delegate void ConnetedHandler(NetConnection net);
    public delegate void DataHandler(ByteArray bytes);
    public delegate void ClosedHandler(NetConnection net);

    public class NetConnection
    {
        private ConnetedHandler connetedhandler;
        private DataHandler datahandler;
        private ClosedHandler closehandler;

        ByteArray readbytes;
        ByteArray writebytes;
        public void Connet(string adress, int port, bool encrypt = false)
        {

        }

        public void Close()
        {

        }

        public void Send(ByteArray bytes)
        {

        }

        public void OnConneted(ConnetedHandler handler)
        {
            this.connetedhandler = handler;
        }
        public void OnData(DataHandler handler)
        {
            this.datahandler = handler;
        }
        public void OnClosed(ClosedHandler handler)
        {
            this.closehandler = handler;
        }
        bool _conneted;
        public bool conneted
        {
            get { return this._conneted; }
        }
    }
}