using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureFunctionsToolkit.Portable.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Org.OpenAPITools.Models;
using src.Subsystems.Analysis;

namespace src.Websockets
{
    [Authorize]
    public class WebsocketController: WebsocketControllerAbstract
    {
        private readonly IAnalysisService _analysisService;

        public WebsocketController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }
        
        public override async Task Get()
        {
            ConfigureStorageManager();
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await 
                    HttpContext.WebSockets.AcceptWebSocketAsync();
                await SendMessage("Connected", "You have connected to the socket server.", "info", webSocket);
                while (webSocket.State == WebSocketState.Open)
                {
                    var request = ReceiveMessage(webSocket).Result;
                    if (request == null)
                    {
                        await SendMessage("Invalid Format", "Request is not structured correctly.", "error", webSocket);
                        continue;
                    }
                    
                    string responseTitle;
                    string responseBody;
                    string responseType;
                    try
                    {
                        switch (request.Request)
                        {
                            case "AnalyzeImage":
                                var analyzedImage = _analysisService.AnalyzeImage(request).Result;
                                if (analyzedImage == null)
                                {
                                    responseTitle = "Image Analysis Error";
                                    responseBody = "Invalid pipeline- or media id provided.";
                                    responseType = "error";
                                }
                                else
                                {
                                    responseTitle = "Image Analyzed";
                                    responseBody = JsonConvert.SerializeObject(analyzedImage);
                                    responseType = "success";
                                }
                                break;
                            case "AnalyzeVideo":
                                var analyzedVideo = _analysisService.AnalyzeVideo(request).Result;
                                if (analyzedVideo == null)
                                {
                                    responseTitle = "Video Analysis Error";
                                    responseBody = "Invalid pipeline- or media id provided.";
                                    responseType = "error";
                                }
                                else
                                {
                                    responseTitle = "Video Analyzed";
                                    responseBody = JsonConvert.SerializeObject(analyzedVideo);
                                    responseType = "success";
                                }
                                break;
                            case "Exit":
                                await SendMessage("Socket Closed", "Connection to the socket was closed.", "info",
                                    webSocket);
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket closed",
                                    CancellationToken.None);
                                continue;
                            default:
                                await SendMessage("Invalid Format", "Invalid request parameter set.", "error",
                                    webSocket);
                                continue;
                        }
                    }
                    catch (JsonSerializationException)
                    {
                        await SendMessage("Invalid Format", "Invalid request body.", "error", webSocket);
                        continue;
                    }
                    catch (Exception e)
                    {
                        await SendMessage("Internal Error", e.InnerException.ToString(), "error", webSocket);
                        continue;
                    }

                    await SendMessage(responseTitle, responseBody, responseType, webSocket);

                }
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
        
        private static async Task SendMessage(string title, string message, string type, WebSocket webSocket)
        {
            var payload = "{\"title\": \"" + title + "\",\"message\": \"" + message + "\",\"type\": \"" + type + "\"}";
            var buffer = Encoding.Default.GetBytes(payload);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private static async Task<SocketRequest> ReceiveMessage(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var requestString = Encoding.Default.GetString(buffer);
            try
            {
                var socketRequest = JsonConvert.DeserializeObject<SocketRequest>(requestString);
                return socketRequest;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private void ConfigureStorageManager()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            _analysisService.SetBaseContainer(jsonToken.Subject);
        }
    }
}