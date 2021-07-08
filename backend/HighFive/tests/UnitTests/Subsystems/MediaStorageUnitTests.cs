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
        public MediaStorageUnitTests()
        {
            _mockMediaStorageService = new MediaStorageService(new StorageManager());
            ((MediaStorageService) _mockMediaStorageService).SetContainer("demo2videomocks");
        }

        [Fact]
        public void TestStoreValidVideo()
        {
            var videoCountBeforeInsert = _mockMediaStorageService.GetAllVideos().Count/2.1;
            var validVideo = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validVideo", "validVideo");
            _mockMediaStorageService.StoreVideo(validVideo);
            var videoCountAfterInsert = _mockMediaStorageService.GetAllVideos().Count/2.0;
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
        public void TestGetVideoValidVideoId()
        {
            var validVideoId = "B6BC3145D0D61BC932AACDA4FBB08FB4";
            var request = new GetVideoRequest
            {
                Id = validVideoId
            };
            var response = _mockMediaStorageService.GetVideo(request);
            Assert.NotNull(response);
        }
        
        [Fact]
        public void TestGetVideoInvalidVideoId()
        {
            const string validVideoId = "5";
            var request = new GetVideoRequest
            {
                Id = validVideoId
            };
            var response = _mockMediaStorageService.GetVideo(request);
            Assert.Null(response);
        }
        
    }
}