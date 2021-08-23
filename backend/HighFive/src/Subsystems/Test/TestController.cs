using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;
using src.AnalysisTools.VideoDecoder;

namespace src.Subsystems.Test
{
    [Authorize(Policy = "Admin")]
    public class TestController: TestApiController
    {
        public override IActionResult Echo(EchoRequest echoRequest)
        {
            var response = new PingResponse() {Message = echoRequest.Message};
            return StatusCode(200, response);
        }

        public override IActionResult Ping()
        {
            var response = new PingResponse() {Message = "The server is working."};
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return StatusCode(200, response);
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            response.Message += " User id: " + jsonToken.Subject;

            return StatusCode(200, response);
        }
        
    }
}