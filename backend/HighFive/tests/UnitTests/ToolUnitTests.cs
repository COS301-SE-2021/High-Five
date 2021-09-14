using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.Admin;
using src.Subsystems.Tools;
using Xunit;

namespace tests.UnitTests
{
    [Trait("Category","UnitTests")]
    public class ToolUnitTests
    {
        private readonly IToolService _mockToolService;

        public ToolUnitTests()
        {
            var mockStorageManager = new MockStorageManager(new MockAdminValidator());
            _mockToolService = new ToolService(mockStorageManager);

            var toolsFile = mockStorageManager.CreateNewFile("tools.txt", "").Result;
            toolsFile.UploadText("");
            
        }
        
        [Fact]
        public async Task TestUploadAnalysisToolValidModelValidCode()
        {
            var validFile = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "tool", "tool.cs");
            var response =await _mockToolService.UploadAnalysisTool(validFile, validFile, "BoxCoordinates", "MyTool");
            
            Assert.NotNull(response);
            Assert.Equal("MyTool", response.ToolName);
        }
        
        [Fact]
        public async Task TestUploadAnalysisToolNullModelValidCode()
        {
            var validFile = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "tool", "tool.cs");
            var response =await _mockToolService.UploadAnalysisTool(validFile, null, "BoxCoordinates", "MyTool");
            
            Assert.Null(response);
        }
        
        [Fact]
        public async Task TestUploadAnalysisToolNullModelNullCode()
        {
            var response =await _mockToolService.UploadAnalysisTool(null, null, "BoxCoordinates", "MyTool");
            
            Assert.Null(response);
        }
        
        [Fact]
        public async Task TestUploadAnalysisToolValidModelNullCode()
        {
            var validFile = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "tool", "tool.cs");
            var response =await _mockToolService.UploadAnalysisTool(null, validFile, "BoxCoordinates", "MyTool");
            
            Assert.Null(response);
        }
        
        [Fact]
        public async Task TestUploadDrawingToolValidCode()
        {
            var validFile = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "tool", "tool.cs");
            var response =await _mockToolService.UploadDrawingTool(validFile, "BoxCoordinates", "MyTool");
            
            Assert.NotNull(response);
            Assert.Equal("MyTool", response.ToolName);
        }
        
        [Fact]
        public async Task TestUploadDrawingToolNullCode()
        {
            var response =await _mockToolService.UploadDrawingTool(null, "BoxCoordinates", "MyTool");
            
            Assert.Null(response);
        }
        
        [Fact]
        public async Task TestGetAllTools()
        {
            var validFile = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "tool", "tool.cs");
            await _mockToolService.UploadAnalysisTool(validFile, validFile, "BoxCoordinates", "MyTool");
            var response = _mockToolService.GetAllTools();
            
            Assert.NotEmpty(response);
            Assert.Equal(6, response.Count);
        }
        
        [Fact]
        public void TestGetToolTypes()
        {
            var response =  _mockToolService.GetToolTypes();
            
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count);
        }
    }
}