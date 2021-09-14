using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Org.OpenAPITools.Models;
using Xunit;

namespace tests.IntegrationTests
{
    public class ToolIntegrationTests
    {
        private TestServer _server;
        private HttpClient _client;

        public ToolIntegrationTests()
        {
            /*
             *      Description:
             * The constructor for this integration testing class instantiates the server and a client
             * through ASP.NET Core's built-in webhost.
             */

            _server = new TestServer(new WebHostBuilder()
                .UseTestServer()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
        }
        
        [Fact]
        public async Task TestUploadAnalysisToolValidModelValidCode()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "/IntegrationTests/Setup/MockImage.jpeg");
            var streamContent = new StreamContent(file);
            var stringContentName = new StringContent("MyTool");
            var stringContentMetadataType = new StringContent("BoxCoordinates");
            stringContentName.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            stringContentMetadataType.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var storeRequest = new MultipartFormDataContent
            {
                {streamContent, "sourceCode", "code.cs"},
                {streamContent, "model", "model.onnx"},
                {stringContentName,"toolName","BoxCoordinates"},
                {stringContentMetadataType, "metadataType", "MyTool"}
            };
            var response = await _client.PostAsync("/tools/uploadAnalysisTool", storeRequest);
            var responseBody = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task TestUploadAnalysisToolNullModelValidCode()
        {

        }
        
        [Fact]
        public async Task TestUploadAnalysisToolNullModelNullCode()
        {

        }
        
        [Fact]
        public async Task TestUploadAnalysisToolValidModelNullCode()
        {

        }
        
        [Fact]
        public async Task TestUploadDrawingToolValidCode()
        {

        }
        
        [Fact]
        public async Task TestUploadDrawingToolNullCode()
        {

        }
        
        [Fact]
        public async Task TestGetAllTools()
        {

        }
        
        [Fact]
        public void TestGetToolTypes()
        {

        }
    }
}