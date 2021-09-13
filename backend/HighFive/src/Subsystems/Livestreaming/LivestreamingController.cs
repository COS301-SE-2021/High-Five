using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Accord.Math;
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
        private bool _applicationCreated;
        private string _appName;

        public LivestreamingController(ILivestreamingService livestreamingService)
        {
            _livestreamingService = livestreamingService;
        }
        
        public override IActionResult CreateOneTimeToken(CreateOneTimeTokenRequest request)
        {
            if (!_applicationCreated)
            {
                var created = CreateUserStreamingApplication().Result;
            }

            var response = new EmptyObject
            {
                Success = true,
                Message = _livestreamingService.CreateOneTimeToken(_appName, request.StreamingId, "play").Result
            };
            return StatusCode(200, response);
        }

        public override IActionResult ReturnAllLiveStreams()
        {
            if (!_applicationCreated)
            {
                var created = CreateUserStreamingApplication().Result;
            }
            throw new System.NotImplementedException();
        }

        private async Task<bool> CreateUserStreamingApplication()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return false;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            var test = _livestreamingService.AuthenticateUser().Result;
            _appName = _livestreamingService.CreateApplication(jsonToken.Subject).Result;
            await _livestreamingService.UpdateApplicationSettings(_appName);
            _applicationCreated = true;
            return true;
        }
    }
}