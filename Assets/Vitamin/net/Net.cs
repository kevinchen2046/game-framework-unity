using System;
using System.Collections;
namespace vitamin
{
    public interface IUpMsg
    {
        int routId { get; }
        int __id__ { get; }
        object data { get; set; }

        ByteArray bytes{ get; }
    }
    public interface IDownMsg
    {
        int routId { get; }
        int __id__ { get; }
        object data { get; set; }
    }
    public delegate void MessageHandler(Net net);
    public class Net
    {
        Connection connection;
        Hashtable requests;
        uint reqId;
        public Net()
        {
            connection = new Connection();
            connection.OnData(dataHandler);
            requests=new Hashtable();
            reqId=0;
        }
        public void Connet(string adress, int port, bool encrypt = false)
        {

        }
        public void Request(IUpMsg msg, Action<object> method)
        {
            NetRequest request=new NetRequest(reqId,msg,method);
            requests.Add(reqId,request);
            connection.Send(msg.bytes);
            reqId++;
            // Vitamin.delay(1000, (object sender, System.Timers.ElapsedEventArgs arg) =>
            //  {
            //      method(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            //  });
        }
        public void Notify(IUpMsg msg)
        {
            connection.Send(msg.bytes);
        }

        private void dataHandler(ByteArray bytes)
        {
            uint msg = bytes.ReadUnsignedShort();
            uint reqId = bytes.ReadUnsignedShort();
            if(reqId==0){
                //Notify
                
            }else{
                //Request
                foreach(NetRequest request in requests){
                    if(request.requestId==reqId){
                        //request.action.Invoke()    
                        break;
                    }
                }
            }
        }
        public void OnConneted(ConnetedHandler handler)
        {
            this.connection.OnConneted(handler);
        }
        public void OnClosed(ClosedHandler handler)
        {
            this.connection.OnClosed(handler);
        }
        public bool conneted
        {
            get { return connection.conneted; }
        }
    }
}