namespace src.Websockets
{
    public class SocketRequest
    {
        public string Authorization { get; set; }
        public string Request { get; set; }
        public object Body { get; set; }
    }
}