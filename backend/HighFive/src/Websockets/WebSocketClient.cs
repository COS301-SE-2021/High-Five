using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace src.Websockets
{
    public class WebSocketClient
    {
        private ClientWebSocket _socket;

        public async Task Connect(string uri)
        {
            await _socket.ConnectAsync(new Uri(uri), CancellationToken.None); ;
        }

        public async Task Send(string data)
        {
            await _socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<string> Receive()
        {
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
    }
}