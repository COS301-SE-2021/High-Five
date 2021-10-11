using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Org.OpenAPITools.Models;
using Xunit;

namespace tests.IntegrationTests
{
    [Trait("Category","IntegrationTests")]
    public class UserIntegrationTests
    {
        private TestServer _server;
        private HttpClient _client;

        public UserIntegrationTests()
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
        public async Task TestGetAllUsers()
        {
            var response = await _client.GetAsync("/users/getAllUsers");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetAllUsersResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseObject.Users);
            Assert.Equal(3, responseObject.Users.Count);
        }
        
        [Fact]
        public async Task TestDeleteMedia()
        {
            var mediaCountBeforePurge = UploadImage().Result;
            var request = new UserRequest {Id = "U1"};
            
            var response = await _client.PostAsync("/users/deleteOwnMedia", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetAllUsersResponse>(responseBody);

            var mediaCountAfterPurge = GetImageCount().Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(mediaCountBeforePurge, mediaCountAfterPurge);
            Assert.Equal(0, mediaCountAfterPurge);
        }
        
        [Fact]
        public async Task TestIsAdminOnNonAdmin()
        {
            var response = await _client.GetAsync("/users/isAdmin");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<IsAdminResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(responseObject.IsAdmin);
        }
        
        [Fact]
        public async Task TestIsAdminOnAdmin()
        {
            var response = await _client.GetAsync("/users/isAdmin");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<IsAdminResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!responseObject.IsAdmin);
        }
        
        [Fact]
        public async Task RevokeAdminOnNonAdmin()
        {
            var request = new UserRequest {Id = "U1"};
            var response = await _client.PostAsync("/users/revokeAdmin", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<IsAdminResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(responseObject.IsAdmin);
        }
        
        [Fact]
        public async Task RevokeAdminOnAdmin()
        {
            var request = new UserRequest {Id = "U3"};
            var response = await _client.PostAsync("/users/revokeAdmin", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<IsAdminResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(responseObject.IsAdmin);
        }
        
        [Fact]
        public async Task TestUpgradeToAdminOnNonAdmin()
        {
            var request = new UserRequest {Id = "U1"};
            var response = await _client.PostAsync("/users/upgradeToAdmin", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<IsAdminResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(responseObject.IsAdmin);
        }
        
        [Fact]
        public async Task TestUpgradeToAdminOnAdmin()
        {
            var request = new UserRequest {Id = "U3"};
            var response = await _client.PostAsync("/users/upgradeToAdmin", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<IsAdminResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(responseObject.IsAdmin);
        }
        
        private async Task<int> UploadImage()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "/IntegrationTests/Setup/MockImage.jpeg");
            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var storeRequest = new MultipartFormDataContent {{streamContent, "file", "MockImage.jpeg"}};
            await _client.PostAsync("/media/storeImage", storeRequest);

            return GetImageCount().Result;
        }

        private async Task<int> GetImageCount()
        {
            var response = await _client.GetAsync("/media/getAllImages");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetAllImagesResponse>(responseBody);
            return responseObject.Images.Count;
        }
        
        private ByteArrayContent ObjectToBytes(object requestObject)
        {
            var jsonRequest = JsonConvert.SerializeObject(requestObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return byteContent;
        }
        
    }
}