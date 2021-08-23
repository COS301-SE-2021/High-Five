using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;

namespace src.Subsystems.User
{
    [Authorize(Policy = "Admin")]
    public class UserController: UserApiController
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        public override IActionResult DeleteMedia()
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult GetAllUsers()
        {
            throw new System.NotImplementedException();
        }
    }
}