using System.Collections.Generic;
using System.Threading.Tasks;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.Pipelines;
using src.Subsystems.Tools;
using Xunit;

namespace tests.UnitTests
{
    [Trait("Category","UnitTests")]
    public class PipelinesUnitTest
    {
        private readonly IPipelineService _mockPipelineService;
        public PipelinesUnitTest()
        {
            var mockStorageManager = new MockStorageManager();
            _mockPipelineService = new PipelineService(mockStorageManager, new ToolService(mockStorageManager));
        }

        [Fact]
        public void TestGetPipelines()
        {
            var pipelines = _mockPipelineService.GetPipelines().Pipelines;
            Assert.Empty(pipelines);
        }

        [Fact]
        public async Task TestCreatePipeline()
        {
            var newPipeline = new NewPipeline()
            {
                Name = "New Pipeline",
                Tools = new List<string>()
            };
            var request = new CreatePipelineRequest
            {
                Pipeline = newPipeline
            };
            var pipelineCountBeforeInsert = _mockPipelineService.GetPipelines().Pipelines.Count;
            await _mockPipelineService.CreatePipeline(request);
            var pipelineCountAfterInsert = _mockPipelineService.GetPipelines().Pipelines.Count;
            Assert.NotEqual(pipelineCountBeforeInsert, pipelineCountAfterInsert);
        }

        [Fact]
        public void TestDeleteValidPipeline()
        {
            var validId = GetValidPipelineId().Result;
            var request = new DeletePipelineRequest
            {
                PipelineId = validId
            };
            var response = _mockPipelineService.DeletePipeline(request).Result;
            Assert.True(response);
        }

        [Fact]
        public void TestDeleteInvalidPipeline()
        {
            var invalidId = "5";
            var request = new DeletePipelineRequest
            {
                PipelineId = invalidId
            };
            var response = _mockPipelineService.DeletePipeline(request).Result;
            Assert.False(response);
        }

        /*[Fact]
        public void TestAddToolToValidPipeline()
        {
            var validId = GetValidPipelineId().Result;
            var tools = new List<string> {"newTool"};
            var request = new AddToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.AddTools(request).Result;
            Assert.True(response);
        }*/

        [Fact]
        public void TestAddToolToInvalidPipeline()
        {
            var invalidId = "5";
            var tools = new List<string>();
            tools.Add("newTool");
            var request = new AddToolsRequest
            {
                PipelineId = invalidId,
                Tools = tools
            };
            var response = _mockPipelineService.AddTools(request).Result;
            Assert.False(response);
        }

        [Fact]
        public void TestRemoveExistingToolFromValidPipeline()
        {
            var validId = GetValidPipelineId().Result;
            var tools = new List<string> {"newTool"};
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.RemoveTools(request).Result;
            Assert.True(response);
        }

        [Fact]
        public void TestRemoveExistingToolFromInvalidPipeline()
        {
            var validId = "5";
            var tools = new List<string> {"newTool"};
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.RemoveTools(request).Result;
            Assert.False(response);
        }

        [Fact]
        public void TestRemoveNonexistingToolFromValidPipeline()
        {
            var validId = GetValidPipelineId().Result;
            var tools = new List<string> {"nonExistingTool"};
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.RemoveTools(request).Result;
            Assert.True(response);
        }

        [Fact]
        public void TestRemoveNonexistingToolFromInvalidPipeline()
        {
            var validId = "5";
            var tools = new List<string> {"nonExistingTool"};
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.RemoveTools(request).Result;
            Assert.False(response);
        }
        
        [Fact]
        public async Task TestGetPipelineIds()
        {
            await GetValidPipelineId();
            var pipelineIds = _mockPipelineService.GetPipelines().Pipelines;
            Assert.NotEmpty(pipelineIds);
        }
        
        [Fact]
        public void TestGetPipelineValidId()
        {
            var validId = GetValidPipelineId().Result;
            var request = new GetPipelineRequest
            {
                PipelineId = validId
            };
            var response = _mockPipelineService.GetPipeline(request).Result;
            Assert.NotNull(response);
        }
        
        [Fact]
        public void TestGetPipelineInvalidId()
        {
            var invalidId = "5";
            var request = new GetPipelineRequest
            {
                PipelineId = invalidId
            };
            var response = _mockPipelineService.GetPipeline(request).Result;
            Assert.Null(response);
        }

        private async Task<string> GetValidPipelineId()
        {
            var newPipeline = new NewPipeline()
            {
                Name = "New Pipeline",
                Tools = new List<string>()
            };
            var request = new CreatePipelineRequest
            {
                Pipeline = newPipeline
            };
            await _mockPipelineService.CreatePipeline(request);
            var validId = _mockPipelineService.GetPipelines().Pipelines[0].Id;
            return validId;
        }
    }
}
