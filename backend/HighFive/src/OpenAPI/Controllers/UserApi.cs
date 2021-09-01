/*
 * High Five
 *
 * The OpenAPI specification for High Five's controllers
 *
 * The version of the OpenAPI document: 0.0.1
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Attributes;
using Org.OpenAPITools.Models;

namespace Org.OpenAPITools.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public abstract class UserApiController : ControllerBase
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Delete Media use case</remarks>
        /// <param name="userRequest"></param>
        /// <response code="200">Called by an admin to delete all the media of another user</response>
        [HttpPost]
        [Route("/users/deleteMedia")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract Task<IActionResult> DeleteMedia([FromBody]UserRequest userRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Delete Own Media use case</remarks>
        /// <response code="200">Called by a user to delete all of their own media</response>
        [HttpPost]
        [Route("/users/deleteOwnMedia")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract Task<IActionResult> DeleteOwnMedia();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get All Users use case</remarks>
        /// <response code="200">All users are returned</response>
        [HttpGet]
        [Route("/users/getAllUsers")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetAllUsersResponse))]
        public abstract IActionResult GetAllUsers();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Is Admin use case</remarks>
        /// <response code="200">Used to verify whether the currently logged in user is an admin or not</response>
        [HttpGet]
        [Route("/users/isAdmin")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(IsAdminResposne))]
        public abstract IActionResult IsAdmin();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Revoke Admin use case</remarks>
        /// <param name="userRequest"></param>
        /// <response code="200">Used to remove admin rights from an existing admin</response>
        [HttpPost]
        [Route("/users/revokeAdmin")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(IsAdminResposne))]
        public abstract IActionResult RevokeAdmin([FromBody]UserRequest userRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Upgrade To Admin use case</remarks>
        /// <param name="userRequest"></param>
        /// <response code="200">Called by an admin to upgrade a normal use to an administrator</response>
        [HttpPost]
        [Route("/users/upgradeToAdmin")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult UpgradeToAdmin([FromBody]UserRequest userRequest);
    }
}
