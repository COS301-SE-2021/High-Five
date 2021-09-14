using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;
using src.AnalysisTools.VideoDecoder;
using src.Storage;
using src.Subsystems.Admin;
using src.Subsystems.MediaStorage;
using Xunit;

namespace tests.UnitTests
{
    [Trait("Category","UnitTests")]
    public class MediaStorageUnitTests
    {
        private readonly IMediaStorageService _mockMediaStorageService;
        private readonly IStorageManager _mockStorageManager;
        public MediaStorageUnitTests()
        {
            _mockStorageManager = new MockStorageManager(new MockAdminValidator());
            _mockMediaStorageService = new MediaStorageService(_mockStorageManager, new MockVideoDecoder());
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
        public async Task TestStoreValidImage()
        {
            var imageCountBeforeInsert = _mockMediaStorageService.GetAllImages().Count;
            var validImage = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validImage.png", "validImage.png");
            await _mockMediaStorageService.StoreImage(validImage);
            var imageCountAfterInsert = _mockMediaStorageService.GetAllImages().Count;
            Assert.NotEqual(imageCountBeforeInsert, imageCountAfterInsert);
        }

        [Fact]
        public void TestStoreNullVideo()
        {
            var videoCountBeforeInsert = _mockMediaStorageService.GetAllVideos().Count;
            FormFile invalidVideo = null;
            _mockMediaStorageService.StoreVideo(invalidVideo);
            var imageCountAfterInsert = _mockMediaStorageService.GetAllVideos().Count;
            Assert.Equal(videoCountBeforeInsert, imageCountAfterInsert);
        }
        
        [Fact]
        public async Task TestStoreNullImage()
        {
            var imageCountBeforeInsert = _mockMediaStorageService.GetAllImages().Count;
            FormFile invalidImage = null;
            await _mockMediaStorageService.StoreImage(invalidImage);
            var imageCountAfterInsert = _mockMediaStorageService.GetAllImages().Count;
            Assert.Equal(imageCountBeforeInsert, imageCountAfterInsert);
        }
        
        [Fact]
        public async Task TestStoreImageInvalidExtension()
        {
            var validImage = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validImage", "validImage");
            await Assert.ThrowsAsync<InvalidDataException>(() => _mockMediaStorageService.StoreImage(validImage));
        }

        [Fact]
        public void TestGetAllVideos()
        {
            var response = _mockMediaStorageService.GetAllVideos();
            Assert.NotNull(response);
        }
        
        [Fact]
        public void TestGetAllImages()
        {
            var response = _mockMediaStorageService.GetAllImages();
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
        public void TestDeleteImageValidVideoId()
        {
            var allImages = _mockMediaStorageService.GetAllImages();
            var imageCountBeforeInsert = allImages.Count;
            if (allImages.Count == 0)
            {
                return;
            }
            var validImageId = allImages[0].Id;
            var request = new DeleteImageRequest
            {
                Id = validImageId
            };
            _mockMediaStorageService.DeleteImage(request);
            var imageCountAfterInsert = _mockMediaStorageService.GetAllImages().Count;
            Assert.Equal(imageCountBeforeInsert, imageCountAfterInsert);
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
        public void TestDeleteImageInvalidVideoId()
        {
            var imageCountBeforeInsert = _mockMediaStorageService.GetAllImages().Count;
            var invalidImageId = "5";
            var request = new DeleteImageRequest
            {
                Id = invalidImageId
            };
            _mockMediaStorageService.DeleteImage(request);
            var imageCountAfterInsert = _mockMediaStorageService.GetAllImages().Count;
            Assert.Equal(imageCountBeforeInsert, imageCountAfterInsert);
        }

    }
}
