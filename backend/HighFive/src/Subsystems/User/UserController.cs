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
        public override IActionResult DeleteMedia(UserRequest userRequest)
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult DeleteOwnMedia()
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        [Authorize(Policy = "Admin")]
        public override IActionResult UpgradeToAdmin(UserRequest userRequest)
        {
            _userService.UpgradeToAdmin(userRequest);
            return StatusCode(200, new EmptyObject() {Success = true});
        }
    }
}