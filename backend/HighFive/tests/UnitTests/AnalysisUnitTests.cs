using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using src.AnalysisTools.VideoDecoder;
using src.Storage;
using src.Subsystems.Analysis;
using src.Subsystems.MediaStorage;
using src.Subsystems.Pipelines;
using Xunit;

namespace tests.UnitTests
{
    public class AnalysisUnitTests
    {
        private readonly IAnalysisService _mockAnalysisService;
        private readonly IMediaStorageService _mockMediaStorageService;
        private readonly IPipelineService _mockPipelineService;
        private readonly IStorageManager _mockStorageManager;
        private readonly IVideoDecoder _mockVideoDecoder;
        
        public AnalysisUnitTests()
        {
            _mockVideoDecoder = new MockVideoDecoder();
            _mockStorageManager = new MockStorageManager();
            _mockMediaStorageService = new MediaStorageService(_mockStorageManager, new MockVideoDecoder());
            _mockPipelineService = new PipelineService(_mockStorageManager);
            _mockAnalysisService = new AnalysisService(_mockStorageManager,_mockMediaStorageService, _mockPipelineService, new AnalysisModels(), _mockVideoDecoder);
        }

        [Fact]
        public void AnalyzeExistingImageExistingPipeline()
        {
            
        }

        private async Task<string> GetValidVideoId()
        {
            var validVideo = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validVideo", "validVideo");
            await _mockMediaStorageService.StoreVideo(validVideo);
            var response = _mockMediaStorageService.GetAllVideos();
            return response[0].Id;
        }

        private async Task<string> GetValidImageId()
        {
            var validVideo = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validVideo", "validVideo");
            await _mockMediaStorageService.StoreImage(validVideo);
            var response = _mockMediaStorageService.GetAllImages();
            return response[0].Id;
        }
    }
}