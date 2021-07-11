using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace tests.IntegrationTests
{
    public class MediaStorageControllerTests
    {
        /*
         *      Description:
         * This class runs integration tests on the MediaStorage subsystem controller with various
         * combinations of valid and invalid inputs.
         *
         *      Attributes:
         * -> _server: the test server that will be used to run the integration tests.
         * -> _client: the client that will run the tests on the test server.
         */

        private TestServer _server;
        private HttpClient _client;

        public MediaStorageControllerTests()
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
        [Trait("Category","IntegrationTests")]
        public async Task Test()
        {
            var response = await _client.PostAsync("/test/ping", null!);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseCode = response.StatusCode;
        }
        
        [Fact]
        [Trait("Category","IntegrationTests")]
        public async Task TestStoreValidVideo()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "\\IntegrationTests\\Setup\\MockVideo.mp4");
            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var request = new MultipartFormDataContent {{streamContent, "file", "MockVideo"}};

            var response = await _client.PostAsync("/media/storeVideo", request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        [Trait("Category","IntegrationTests")]
        public async Task TestStoreNullVideo()
        {
            var response = await _client.PostAsync("/media/storeVideo", null!);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestGetAllVideos()
        {
            
        }
        
        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestGetExistingVideo()
        {
            
        }
        
        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestGetNonExistingVideo()
        {
            
        }
        
        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestDeleteExistingVideo()
        {
            
        }
        
        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestDeleteNonExistingVideo()
        {
            
        }

        private async Task UploadVideo()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "\\IntegrationTests\\Setup\\MockVideo.mp4");
            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var request = new MultipartFormDataContent {{streamContent, "file", "MockVideo"}};

            var response = await _client.PostAsync("/media/storeVideo", request);
        }
    }
}