using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace src.Websockets
{
    public class WebSocketClient: IWebSocketClient
    {
        private ClientWebSocket _socket = null;
        private static int _id = 0;
        private int _myId;

        public async Task Connect(string uri, string userId)
        {
            if (_socket == null)
            {
                _myId = GetId();
                _socket = new ClientWebSocket();
                await _socket.ConnectAsync(new Uri(uri), CancellationToken.None);
                Console.WriteLine("Socket connected to Broker!");
                await Send(userId);
                var ack = Receive().Result;
            }
        }

        private static int GetId()
        {
            return _id++;
        }
        
        public async Task Send(string data)
        {
            Console.WriteLine("Sending from socket" + _myId + ", data: " + data);
            await _socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<string> Receive()
        {
            while (_socket == null)
            {
            }

            var buffer = new ArraySegment<byte>(new byte[2048]);
            var received = string.Empty;
            while (received == string.Empty)
            {
                WebSocketReceiveResult result;
                await using var ms = new MemoryStream();
                do
                {
                    result = await _socket.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                ms.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(ms, Encoding.UTF8);
                received = await reader.ReadToEndAsync();
            }

            return received;
        }

        public void Close()
        {
            _socket.CloseAsync(WebSocketCloseStatus.Empty, null, CancellationToken.None);
        }
    }
}