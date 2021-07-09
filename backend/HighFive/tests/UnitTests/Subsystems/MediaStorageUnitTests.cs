using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.MediaStorage;
using src.Utils.Controller;
using Xunit;

namespace tests.UnitTests.Subsystems
{
    public class MediaStorageUnitTests 
    {
        private readonly IMediaStorageService _mockMediaStorageService;
        private readonly IStorageManager _mockStorageManager;
        public MediaStorageUnitTests()
        {
            _mockStorageManager = new MockStorageManager();
            _mockMediaStorageService = new MediaStorageService(_mockStorageManager);
        }

        [Fact]
        public async Task TestStoreValidVideo()
        {
            var videoCountBeforeInsert = _mockMediaStorageService.GetAllVideos().Count;
            var validVideo = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validVideo", "validVideo");
            await _mockMediaStorageService.StoreVideo(validVideo);
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Count;
            Assert.NotEqual(videoCountBeforeInsert, videoCountAfterInsert);
        }
        
        [Fact]
        public void TestStoreNullVideo()
        {
            var videoCountBeforeInsert = _mockMediaStorageService.GetAllVideos().Count;
            FormFile invalidVideo = null;
            _mockMediaStorageService.StoreVideo(invalidVideo);
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Count;
            Assert.Equal(videoCountBeforeInsert, videoCountAfterInsert);
        }

        [Fact]
        public void TestGetAllVideos()
        {
            var response = _mockMediaStorageService.GetAllVideos();
            Assert.NotNull(response);
        }
        
        [Fact]
        public void TestDeleteVideoValidVideoId()
        {
            var allVids = _mockMediaStorageService.GetAllVideos();
            var videoCountBeforeInsert = allVids.Count;
            if (allVids.Count == 0)
            {
                return;
            }
            var validVideoId = allVids[0].Id;
            var request = new DeleteVideoRequest
            {
                Id = validVideoId
            };
            _mockMediaStorageService.DeleteVideo(request);
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Count;
            Assert.Equal(videoCountBeforeInsert, videoCountAfterInsert);
        }
        
        [Fact]
        public void TestDeleteVideoInvalidVideoId()
        {
            var videoCountBeforeInsert = _mockMediaStorageService.GetAllVideos().Count;
            var invalidVideoId = "5";
            var request = new DeleteVideoRequest
            {
                Id = invalidVideoId
            };
            _mockMediaStorageService.DeleteVideo(request);
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Count;
            Assert.Equal(videoCountBeforeInsert, videoCountAfterInsert);
        }
        
        [Fact]
        public async Task TestGetVideoValidVideoId()
        {
            var validVideo = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validVideo", "validVideo");
            await _mockMediaStorageService.StoreVideo(validVideo);
            var validVideoId = _mockMediaStorageService.GetAllVideos()[0].Id;
            var getController = new GetVideoController(_mockStorageManager);
            var response = getController.GetVideo(validVideoId);
            Assert.NotEmpty(response.FileContents);
        }
        
        [Fact]
        public void TestGetVideoInvalidVideoId()
        {
            const string invalidVideoId = "5";
            var getController = new GetVideoController(_mockStorageManager);
            var response = getController.GetVideo(invalidVideoId);
            Assert.Empty(response.FileContents);
        }
        
    }
}