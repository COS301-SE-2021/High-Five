using System.ComponentModel;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace tests.IntegrationTests
{
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

        [Trait("Category","IntegrationTests")]
        [Fact]
        public void TestCreatePipeline()
        {
            
        }

        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestGetAllPipelines()
        {
            
        }

        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestGetAllTools()
        {
            
        }

        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestRemoveToolsFromExistingPipeline()
        {
            
        }

        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestRemoveToolsFromNonExistingPipeline()
        {
            
        }

        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestAddToolsToExistingPipeline()
        {
            
        }

        [Fact]
        [Trait("Category","IntegrationTests")]
        public void TestAddToolsToNonExistingPipeline()
        {
            
        }
        
    }
}