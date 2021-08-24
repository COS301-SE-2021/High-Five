using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.User
{
    [Authorize]
    public class UserController: UserApiController
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = "Admin")]
        public override async Task<IActionResult> DeleteMedia(UserRequest userRequest)
        {
            await _userService.DeleteMedia(userRequest);
            return StatusCode(200, new EmptyObject {Success = true});
        }

        public override async Task<IActionResult> DeleteOwnMedia()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return StatusCode(200, null);
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            await _userService.DeleteMedia(new UserRequest{Id = jsonToken.Subject});
            return StatusCode(200, new EmptyObject {Success = true});
        }

        [Authorize(Policy = "Admin")]
        public override IActionResult DeleteUser(UserRequest userRequest)
        {
            throw new System.NotImplementedException();
        }

        [Authorize(Policy = "Admin")]
        public override IActionResult GetAllUsers()
        {
            var response = _userService.GetAllUsers();
            return StatusCode(200, response);
        }

        [Authorize(Policy = "Admin")]
        public override IActionResult UpgradeToAdmin(UserRequest userRequest)
        {
            _userService.UpgradeToAdmin(userRequest);
            return StatusCode(200, new EmptyObject() {Success = true});
        }
    }
}