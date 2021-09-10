using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;

namespace src.Subsystems.FileDownloads
{
    public class DownloadsController: DownloadsApiController
    {
        private readonly IDownloadsService _downloadsService;
        private bool _baseContainerSet;

        public DownloadsController(IDownloadsService downloadsService)
        {
            _downloadsService = downloadsService;
            _baseContainerSet = false;
        }
        
        public override IActionResult DownloadApk()
        {
            return StatusCode(503, null);
            /*var response = _downloadsService.DownloadApk();
            return StatusCode(200, response);*/
        }

        public override IActionResult DownloadSdkFiles()
        {
            return StatusCode(503, null);
            /*var response = _downloadsService.DownloadSdkFiles();
            return StatusCode(200, response);*/
        }

        public override IActionResult DownloadSdkManual()
        {
            return StatusCode(503, null);
            /*var response = _downloadsService.DownloadSdkManual();
            return StatusCode(200, response);*/
        }
    }
}