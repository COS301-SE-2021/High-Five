using System.Collections.Generic;
using System.ComponentModel;
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
    public class PipelinesIntegrationTests
    {
        /*
        *      Description:
        * This class runs integration tests on the Pipelines subsystem controller with various
        * combinations of valid and invalid inputs.
        *
        *      Attributes:
        * -> _server: the test server that will be used to run the integration tests.
        * -> _client: the client that will run the tests on the test server.
        */
        
        private TestServer _server;
        private HttpClient _client;
        
        public PipelinesIntegrationTests()
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
        public async Task TestCreatePipeline()
        {
            var initialTools = new List<string> {"CarCounting", "CarRecognition"};
            var mockPipeline = new NewPipeline
            {
                Name = "MockPipeline",
                Tools = initialTools
            };
            var request = new CreatePipelineRequest { Pipeline = mockPipeline };

            var response = await _client.PostAsync("/pipelines/createPipeline", ObjectToBytes(request));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestGetAllPipelines()
        {
            await GetPipelineId();

            var response = await _client.PostAsync("/pipelines/getPipelines", null!);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetPipelinesResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseObject.Pipelines);
        }

        [Fact]
        public async Task TestGetAllTools()
        {
            var response = await _client.PostAsync("/pipelines/getAllTools", null!);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<List<string>>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseObject);
        }

        [Fact]
        public async Task TestRemoveToolsFromExistingPipeline()
        {
            var tools = new List<string> {"CarRecognition", "CowRecognition"};
            var validId = GetPipelineId().Result;
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };

            var response = await _client.PostAsync("/pipelines/removeTools", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<EmptyObject>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseObject.Success);
        }

        [Fact]
        public async Task TestRemoveToolsFromNonExistingPipeline()
        {
            var tools = new List<string> {"CarRecognition", "CowRecognition"};
            var invalidId = "123";
            var request = new RemoveToolsRequest
            {
                PipelineId = invalidId,
                Tools = tools
            };

            var response = await _client.PostAsync("/pipelines/removeTools", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<EmptyObject>(responseBody);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(responseObject.Success);
        }

        [Fact]
        public async Task TestAddToolsToExistingPipeline()
        {
            var tools = new List<string> {"CarRecognition", "CowRecognition"};
            var validId = await GetPipelineId();
            var request = new AddToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };

            var response = await _client.PostAsync("/pipelines/addTools", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<EmptyObject>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseObject.Success);
        }

        [Fact]
        public async Task TestAddToolsToNonExistingPipeline()
        {
            var tools = new List<string> {"CarRecognition", "CowRecognition"};
            var invalidId = "123";
            var request = new AddToolsRequest
            {
                PipelineId = invalidId,
                Tools = tools
            };

            var response = await _client.PostAsync("/pipelines/addTools", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<EmptyObject>(responseBody);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(responseObject.Success);
        }
        
        [Fact]
        public async Task TestGetPipelineIds()
        {
            await GetPipelineId();
            var response = await _client.PostAsync("/pipelines/getPipelineIds", null!);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<GetPipelineIdsResponse>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseObject.PipelineIds);
        }
        
        [Fact]
        public async Task TestGetPipelineValidId()
        {
            var validId = GetPipelineId().Result;
            var request = new GetPipelineRequest
            {
                PipelineId = validId
            };

            var response = await _client.PostAsync("/pipelines/getPipeline", ObjectToBytes(request));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task TestGetPipelineInvalidId()
        {
            var invalidId = "5";
            var request = new GetPipelineRequest
            {
                PipelineId = invalidId
            };

            var response = await _client.PostAsync("/pipelines/getPipeline", ObjectToBytes(request));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private ByteArrayContent ObjectToBytes(object requestObject)
        {
            var jsonRequest = JsonConvert.SerializeObject(requestObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return byteContent;
        }

        private async Task<string> GetPipelineId()
        {
            var initialTools = new List<string> {"CarCounting", "CarRecognition"};
            var mockPipeline = new NewPipeline
            {
                Name = "MockPipeline",
                Tools = initialTools
            };
            var request = new CreatePipelineRequest { Pipeline = mockPipeline };

            var response = await _client.PostAsync("/pipelines/createPipeline", ObjectToBytes(request));
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<CreatePipelineResponse>(responseBody);
            return responseObject.Pipeline.Id;
        }
    }
}