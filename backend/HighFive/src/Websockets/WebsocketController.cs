using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureFunctionsToolkit.Portable.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.OpenAPITools.Models;
using src.Subsystems.Analysis;

namespace src.Websockets
{
    public class WebsocketController: WebsocketControllerAbstract
    {
        private readonly IAnalysisService _analysisService;
        private readonly IConfiguration _configuration;
        private bool _baseContainerSet;

        public WebsocketController(IAnalysisService analysisService, IConfiguration configuration)
        {
            _analysisService = analysisService;
            _configuration = configuration;
            _baseContainerSet = false;
        }
        
        public override async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await 
                    HttpContext.WebSockets.AcceptWebSocketAsync();
                await SendMessage("Connected", "You have connected to the socket server.", "info", webSocket);
                ListenForBrokerMessage(webSocket);
                while (webSocket.State == WebSocketState.Open)
                {
                    var responseTitle = string.Empty;
                    var responseBody = string.Empty;
                    var responseType= string.Empty;
                    try
                    {
                        var request = ReceiveMessage(webSocket).Result;
                        ConfigureStorageManager(request);
                        if (request == null)
                        {
                            await SendMessage("Invalid Format", "Request is not structured correctly.", "error",
                                webSocket);
                            continue;
                        }

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
                            case "StartLiveAnalysis":   //This use case must be called by the application
                                var rtmpUri = _analysisService.StartLiveStream();
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
                    catch (UnauthorizedAccessException)
                    {
                        await SendMessage("Unauthorized", "Invalid jwt provided.", "error", webSocket);
                        continue;
                    }
                    catch (Exception e)
                    {
                        await SendMessage("Internal Error", e.StackTrace, "error", webSocket);
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

        private async Task ListenForBrokerMessage(WebSocket socket)
        {
            /*
             * Listens for messages from the Broker, will only send a message to the
             * front-end if a livestream started notification is pushed through.
             */
            
            while (socket.State != WebSocketState.Closed)
            {
                var message = _analysisService.ListenForMessage();
                if (!message.Contains("Livestream Started"))
                {
                    continue;
                }
                var infoObject = JsonConvert.DeserializeObject<SocketResponse>(message);
                await SendMessage("Livestream Started", infoObject.message, "info", socket);
            }
        }
        
        private void ConfigureStorageManager(SocketRequest request)
        {
            if (_baseContainerSet)
            {
                return;
            }

            _baseContainerSet = true;
            var tokenString = request.Authorization;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            if (!IsJwtValid(handler, tokenString).Result)
            {
                throw new UnauthorizedAccessException();
            }
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            _analysisService.SetBaseContainer(jsonToken.Subject);
        }

        private async Task<bool> IsJwtValid(JwtSecurityTokenHandler handler, string tokenString)
        {
            var token = handler.ReadJwtToken(tokenString);
            var iss = token.Issuer;
            var tfp = token.Payload["tfp"].ToString(); // Sign-in policy name
            var metadataEndpoint = $"{iss}.well-known/openid-configuration?p={tfp}";
            var cm = new ConfigurationManager<OpenIdConnectConfiguration>(metadataEndpoint,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever());
            var discoveryDocument = await cm.GetConfigurationAsync();
            var signingKeys = discoveryDocument.SigningKeys;

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuers = new[] {"https://highfiveactivedirectory.b2clogin.com/" + _configuration["AzureAdB2C:TenantId"] +
                                      "/v2.0/"},
                ValidAudiences = new [] {_configuration["AzureAdB2C:ClientId"]},
                ValidateAudience = true,
                ValidateIssuer = true,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(60),
                IssuerSigningKeys = signingKeys
            };
            try
            {
                handler.ValidateToken(tokenString, validationParameters, out _);
            }
            catch (Exception)
            {
                return false;
            }

            _analysisService.SetBrokerToken(token.Subject);
            return true;
        }
        
    }
}