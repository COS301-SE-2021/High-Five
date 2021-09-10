using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Attributes;
using Org.OpenAPITools.Models;

namespace src.Websockets
{
    [ApiController]
    public abstract class WebsocketControllerAbstract: ControllerBase
    {
        [HttpGet("/ws")]
        public abstract Task Get();
    }
}