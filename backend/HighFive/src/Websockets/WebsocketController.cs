using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace src.Websockets
{
    public class WebsocketController: WebsocketControllerAbstract
    {
        public override async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await 
                    HttpContext.WebSockets.AcceptWebSocketAsync();
                await SendMessage("Connect", "You have connected to the socket server.", "info", webSocket);
                while (webSocket.State == WebSocketState.Open)
                {
                    if (AnalyzeImage)
                    {
                        AnalyzeImage = false;
                        await SendMessage("Image Analyzed", "Your image has successfully been analyzed.", "success", webSocket);
                    }

                    if (AnalyzeVideo)
                    {
                        AnalyzeVideo = false;
                        await SendMessage("Video Analyzed", "Your video has successfully been analyzed.", "success", webSocket);
                    }

                    if (UploadImage)
                    {
                        UploadImage = false;
                        await SendMessage("Image Upload", "Your image has successfully been uploaded.", "success", webSocket);
                    }

                    if (UploadVideo)
                    {
                        UploadVideo = false;
                        await SendMessage("Video Upload", "Your video has successfully been uploaded.", "success", webSocket);
                    }
                }
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
        
        /*private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }*/

        private async Task SendMessage(string title, string message, string type, WebSocket webSocket)
        {
            var payload = "{\"title\": \"" + title + "\",\"message\": \"" + message + "\",\"type\": \"" + type + "\"}";
            var buffer = Encoding.Default.GetBytes(payload);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        
    }
}