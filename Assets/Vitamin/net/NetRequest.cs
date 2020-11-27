using System;
namespace vitamin
{
    public class NetRequest
    {
        public uint requestId;
        public IUpMsg msgup;
        public Action<IDownMsg> action;
        public NetRequest(uint requestId, IUpMsg msg, Action<IDownMsg> method)
        {
            this.requestId = requestId;
            this.msgup = msg;
            this.action = method;
        }
        public ByteArray updata
        {
            get
            {
                return null;
            }
        }
    }
}