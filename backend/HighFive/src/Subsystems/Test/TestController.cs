﻿using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Test
{
    public class TestController: TestApiController
    {
        public override IActionResult Echo(EchoRequest echoRequest)
        {
            PingResponse response = new PingResponse() {Message = echoRequest.Message};
            return StatusCode(200, response);
        }

        public override IActionResult Ping()
        {
            PingResponse response = new PingResponse() {Message = "The server is working."};
            return StatusCode(200, response);
        }
    }
}