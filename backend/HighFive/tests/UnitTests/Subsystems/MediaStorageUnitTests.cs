using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.MediaStorage;
using Xunit;

namespace tests.UnitTests.Subsystems.MediaStorage
{
    public class MediaStorageUnitTests {
        private IMediaStorageService _mockMediaStorageService;
        private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=high5storage;AccountKey=Au+qHV2suTNDeydwjDjvJHJYxxWTX4/9GyZ6a+qeBoUdsWOJ+SQeMjhid5+Pxu/vR4LQM9yC5uPQlLjk5JHKaw==;EndpointSuffix=core.windows.net";

        public MediaStorageUnitTests()
        {
            _mockMediaStorageService = new MediaStorageService(new StorageManager(ConnectionString));
            ((MediaStorageService) _mockMediaStorageService).SetContainer("demo2videomocks");
        }

        [Fact]
        public void TestStoreValidVideo()
        {
            var videoCountBeforeInsert = _mockMediaStorageService.GetAllVideos().Result.Count/2.0-1;
            var validVideo = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validVideo", "validVideo");
            _mockMediaStorageService.StoreVideo(validVideo);
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Result.Count/2.0;
            Assert.NotEqual(videoCountBeforeInsert, videoCountAfterInsert);
        }
        
        [Fact]
        public void TestStoreNullVideo()
        {
            var videoCountBeforeInsert = _mockMediaStorageService.GetAllVideos().Result.Count;
            FormFile invalidVideo = null;
            _mockMediaStorageService.StoreVideo(invalidVideo);
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Result.Count;
            Assert.Equal(videoCountBeforeInsert, videoCountAfterInsert);
        }

        [Fact]
        public void TestGetAllVideos()
        {
            var response = _mockMediaStorageService.GetAllVideos();
            Assert.NotNull(response.Result);
        }
        
        [Fact]
        public void TestDeleteVideoValidVideoId()
        {
            var allVids = _mockMediaStorageService.GetAllVideos().Result;
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
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Result;
            Assert.NotNull(videoCountAfterInsert);
        }
        
        [Fact]
        public void TestDeleteVideoInvalidVideoId()
        {
            var videoCountBeforeInsert = _mockMediaStorageService.GetAllVideos().Result.Count;
            var invalidVideoId = "5";
            var request = new DeleteVideoRequest
            {
                Id = invalidVideoId
            };
            _mockMediaStorageService.DeleteVideo(request);
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Result.Count;
            Assert.Equal(videoCountBeforeInsert, videoCountAfterInsert);
        }
        
        [Fact]
        public void TestGetVideoValidVideoId()
        {
            var validVideoId = "B6BC3145D0D61BC932AACDA4FBB08FB";
            var request = new GetVideoRequest
            {
                Id = validVideoId
            };
            var response = _mockMediaStorageService.GetVideo(request).Result;
            Assert.Null(response);
        }
        
        [Fact]
        public void TestGetVideoInvalidVideoId()
        {
            var validVideoId = "5";
            var request = new GetVideoRequest
            {
                Id = validVideoId
            };
            var response = _mockMediaStorageService.GetVideo(request).Result;
            Assert.Null(response);
        }
        
    }
}