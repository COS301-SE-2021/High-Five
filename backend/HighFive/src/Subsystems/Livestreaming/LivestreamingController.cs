using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Livestreaming
{
    [Authorize]
    public class LivestreamingController: LivestreamApiController
    {
        private readonly ILivestreamingService _livestreamingService;

        public LivestreamingController(ILivestreamingService livestreamingService)
        {
            _livestreamingService = livestreamingService;
        }
        
        public override IActionResult CreateBroadcast(GetToolFilesRequest getToolFilesRequest)
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult CreateStreamingUrl()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return null;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            var test = _livestreamingService.AuthenticateUser().Result;
            _livestreamingService.CreateApplication(jsonToken.Subject);
            throw new System.NotImplementedException();
        }

        public override IActionResult ReturnAllLiveStreams()
        {
            throw new System.NotImplementedException();
        }
    }
}