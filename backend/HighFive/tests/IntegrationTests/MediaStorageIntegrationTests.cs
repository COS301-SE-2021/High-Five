using System.Collections.Generic;
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
    public class MediaStorageIntegrationTests
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

        public MediaStorageIntegrationTests()
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
        public async Task TestStoreValidVideo()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "/IntegrationTests/Setup/MockVideo.mp4");
            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var request = new MultipartFormDataContent {{streamContent, "file", "MockVideo"}};

            var response = await _client.PostAsync("/media/storeVideo", request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task TestStoreValidImage()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "/IntegrationTests/Setup/MockVideo.mp4");
            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var request = new MultipartFormDataContent {{streamContent, "file", "MockImage.jpeg"}};

            var response = await _client.PostAsync("/media/storeImage", request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task TestStoreImageInvalidFormat()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "/IntegrationTests/Setup/MockVideo.mp4");
            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var request = new MultipartFormDataContent {{streamContent, "file", "MockImage"}};

            var response = await _client.PostAsync("/media/storeImage", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task TestStoreNullVideo()
        {
            var response = await _client.PostAsync("/media/storeVideo", null!);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task TestStoreNullImage()
        {
            var response = await _client.PostAsync("/media/storeImage", null!);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task TestGetAllVideos()
        {
            await UploadVideo();
            var response = await _client.PostAsync("/media/getAllVideos", null!);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetAllVideosResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseObject.Videos);
        }
        
        [Fact]
        public async Task TestGetAllImages()
        {
            await UploadImage();
            var response = await _client.PostAsync("/media/getAllImages", null!);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetAllImagesResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseObject.Images);
        }

        [Fact]
        public async Task TestDeleteExistingVideo()
        {
            var validId = await UploadVideo();
            var requestObject = new DeleteVideoRequest {Id = validId};
            var jsonRequest = JsonConvert.SerializeObject(requestObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await _client.PostAsync("/media/deleteVideo", byteContent);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<EmptyObject>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseObject.Success);
        }
        
        [Fact]
        public async Task TestDeleteExistingImage()
        {
            var validId = await UploadImage();
            var requestObject = new DeleteImageRequest {Id = validId};
            var jsonRequest = JsonConvert.SerializeObject(requestObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await _client.PostAsync("/media/deleteImage", byteContent);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<EmptyObject>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseObject.Success);
        }
        
        [Fact]
        public async Task TestDeleteNonExistingVideo()
        {
            var invalidId = "123";
            var requestObject = new DeleteVideoRequest {Id = invalidId};
            var jsonRequest = JsonConvert.SerializeObject(requestObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await _client.PostAsync("/media/deleteVideo", byteContent);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<EmptyObject>(responseBody);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(responseObject.Success);
        }
        
        [Fact]
        public async Task TestDeleteNonExistingImage()
        {
            var invalidId = "123";
            var requestObject = new DeleteImageRequest {Id = invalidId};
            var jsonRequest = JsonConvert.SerializeObject(requestObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await _client.PostAsync("/media/deleteImage", byteContent);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<EmptyObject>(responseBody);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(responseObject.Success);
        }

        private async Task<string> UploadVideo()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "/IntegrationTests/Setup/MockVideo.mp4");
            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var storeRequest = new MultipartFormDataContent {{streamContent, "file", "MockVideo"}};
            await _client.PostAsync("/media/storeVideo", storeRequest);
            
            var response = await _client.PostAsync("/media/getAllVideos", null!);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetAllVideosResponse>(responseBody);
            var validId = responseObject.Videos[0].Id;
            return validId;
        }
        
        private async Task<string> UploadImage()
        {
            var basePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.ToString());
            var file = File.OpenRead(basePath?.FullName + "/IntegrationTests/Setup/MockImage.jpeg");
            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            var storeRequest = new MultipartFormDataContent {{streamContent, "file", "MockImage.jpeg"}};
            await _client.PostAsync("/media/storeImage", storeRequest);
            
            var response = await _client.PostAsync("/media/getAllImages", null!);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetAllImagesResponse>(responseBody);
            var validId = responseObject.Images[0].Id;
            return validId;
        }
    }
}