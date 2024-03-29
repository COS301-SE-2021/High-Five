﻿using System.Threading.Tasks;

namespace src.Websockets
{
    public interface IWebSocketClient
    {
        public Task Connect(string uri, string userId);
        public Task Send(string data);
        public Task<string> Receive();
        public void Close();
    }
}