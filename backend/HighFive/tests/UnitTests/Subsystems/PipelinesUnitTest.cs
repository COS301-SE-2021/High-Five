﻿using System.Collections.Generic;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.MediaStorage;
using src.Subsystems.Pipelines;
using Xunit;

namespace tests.UnitTests.Subsystems.MediaStorage
{
    public class PipelinesUnitTest
    {
        private IPipelineService _mockPipelineService;
        private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=high5storage;AccountKey=Au+qHV2suTNDeydwjDjvJHJYxxWTX4/9GyZ6a+qeBoUdsWOJ+SQeMjhid5+Pxu/vR4LQM9yC5uPQlLjk5JHKaw==;EndpointSuffix=core.windows.net";

        public PipelinesUnitTest()
        {
            _mockPipelineService = new PipelineService(new StorageManager(ConnectionString));
            ((PipelineService) _mockPipelineService).SetContainer("demo2pipelinemocks");
        }
        
        [Fact]
        public void TestGetPipelines()
        {
            var pipelines = _mockPipelineService.GetPipelines().Pipelines;
            Assert.NotNull(pipelines);
        }
        
        [Fact]
        public void TestCreatePipeline()
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
            var pipelineCountBeforeInsert = _mockPipelineService.GetPipelines().Pipelines.Count-1;
            _mockPipelineService.CreatePipeline(request);
            var pipelineCountAfterInsert = _mockPipelineService.GetPipelines().Pipelines.Count;
            Assert.NotEqual(pipelineCountBeforeInsert, pipelineCountAfterInsert);
        }
        
        [Fact]
        public void TestDeleteValidPipeline()
        {
            var validId = GetValidPipelineId();
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
        
        [Fact]
        public void TestAddToolToValidPipeline()
        {
            var validId = GetValidPipelineId();
            var tools = new List<string>();
            tools.Add("newTool");
            var request = new AddToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.AddTools(request);
            Assert.True(response);
        }
        
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
            var response = _mockPipelineService.AddTools(request);
            Assert.False(response);
        }
        
        [Fact]
        public void TestRemoveExistingToolFromValidPipeline()
        {
            var validId = GetValidPipelineId();
            var tools = new List<string>();
            tools.Add("newTool");
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.RemoveTools(request);
            Assert.True(response);
        }
        
        [Fact]
        public void TestRemoveExistingToolFromInvalidPipeline()
        {
            var validId = "5";
            var tools = new List<string>();
            tools.Add("newTool");
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.RemoveTools(request);
            Assert.False(response);
        }
        
        [Fact]
        public void TestRemoveNonexistingToolFromValidPipeline()
        {
            var validId = GetValidPipelineId();
            var tools = new List<string>();
            tools.Add("nonExistingTool");
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.RemoveTools(request);
            Assert.True(response);
        }
        
        [Fact]
        public void TestRemoveNonexistingToolFromInvalidPipeline()
        {
            var validId = "5";
            var tools = new List<string>();
            tools.Add("nonExistingTool");
            var request = new RemoveToolsRequest
            {
                PipelineId = validId,
                Tools = tools
            };
            var response = _mockPipelineService.RemoveTools(request);
            Assert.False(response);
        }

        private string GetValidPipelineId()
        {
            var allPipes = _mockPipelineService.GetPipelines().Pipelines;
            if (allPipes == null)
            {
                return "";
            }
            return allPipes[0].Id;
        }
    }
}