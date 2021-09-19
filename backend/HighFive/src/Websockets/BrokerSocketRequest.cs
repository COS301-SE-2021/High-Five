namespace src.Websockets
{
    public class BrokerSocketRequest: SocketRequest
    {
        public BrokerSocketRequest()
        {
            
        }
        public BrokerSocketRequest(SocketRequest originalReq, string userId)
        {
            UserId = userId;
            Body = originalReq.Body;
            Request = originalReq.Request;
        }
        
        public string UserId { get; set; }
    }
}