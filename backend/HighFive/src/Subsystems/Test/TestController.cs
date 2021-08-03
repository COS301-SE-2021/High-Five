using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Test
{
    [Authorize]
    public class TestController: TestApiController
    {
        public override IActionResult Echo(EchoRequest echoRequest)
        {
            var response = new PingResponse() {Message = echoRequest.Message};
            return StatusCode(200, response);
        }

        public override IActionResult Ping()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            var response = new PingResponse() {Message = "The server is working. Sub: " + jsonToken.Subject};
            return StatusCode(200, response);
        }
        
    }
}