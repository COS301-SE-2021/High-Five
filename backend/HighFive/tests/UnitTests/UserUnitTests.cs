using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Accord.Math;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;
using src.AnalysisTools.VideoDecoder;
using src.Storage;
using src.Subsystems.Admin;
using src.Subsystems.MediaStorage;
using src.Subsystems.User;
using Xunit;

namespace tests.UnitTests
{
    [Trait("Category","UnitTests")]
    public class UserUnitTests
    {
        private readonly IUserService _mockUserService;
        private readonly IMediaStorageService _mockMediaStorageService;

        public UserUnitTests()
        {
            var mockAdminValidator = new MockAdminValidator();
            var mockStorageManager = new MockStorageManager(mockAdminValidator);
            _mockUserService = new UserService(mockStorageManager, mockAdminValidator);
            _mockMediaStorageService = new MediaStorageService(mockStorageManager,new MockVideoDecoder());
        }

        [Fact]
        public async Task TestDeleteAllMedia()
        {
            var validVideo = new FormFile(new FileStream(Path.GetTempFileName(),FileMode.Create), 0, 1, "validVideo", "validVideo");
            await _mockMediaStorageService.StoreVideo(validVideo);
            var videoCountBeforePurge = _mockMediaStorageService.GetAllVideos().Count;
            var request = new UserRequest {Id = "U1"};
            await _mockUserService.DeleteMedia(request);
            var videoCountAfterPurge = _mockMediaStorageService.GetAllVideos().Count;
            Assert.NotEqual(videoCountAfterPurge, videoCountBeforePurge);
            Assert.Equal(0, videoCountAfterPurge);
        }
        
        [Fact]
        public void TestGetAllUsers()
        {
            var userList =_mockUserService.GetAllUsers();
            Assert.NotEmpty(userList.Users);
            Assert.Equal(3, userList.Users.Count);
        }

        [Fact]
        public void TestIsAdminOnAdmin()
        {
            var isAdmin = _mockUserService.IsAdmin("U3");
            Assert.True(isAdmin);
        }
        
        [Fact]
        public void TestIsAdminOnNonAdmin()
        {
            var isAdmin = _mockUserService.IsAdmin("U1");
            Assert.False(isAdmin);
        }

        [Fact]
        public void TestUpgradeToAdminOnNonAdmin()
        {
            var isAdminBeforeUpgrade = _mockUserService.IsAdmin("U1");
            var userRequest = new UserRequest {Id = "U1"};
            _mockUserService.UpgradeToAdmin(userRequest);
            var isAdminAfterUpgrade = _mockUserService.IsAdmin("U1");
            Assert.False(isAdminBeforeUpgrade);
            Assert.True(isAdminAfterUpgrade);
            _mockUserService.RevokeAdmin(userRequest);
        }
        
        [Fact]
        public void TestUpgradeToAdminOnAdmin()
        {
            var isAdminBeforeUpgrade = _mockUserService.IsAdmin("U3");
            var userRequest = new UserRequest {Id = "U3"};
            _mockUserService.UpgradeToAdmin(userRequest);
            var isAdminAfterUpgrade = _mockUserService.IsAdmin("U3");
            Assert.True(isAdminBeforeUpgrade);
            Assert.True(isAdminAfterUpgrade);
        }
        
        [Fact]
        public void TestRevokeAdminOnAdmin()
        {
            var isAdminBeforeUpgrade = _mockUserService.IsAdmin("U3");
            var userRequest = new UserRequest {Id = "U3"};
            _mockUserService.RevokeAdmin(userRequest);
            var isAdminAfterUpgrade = _mockUserService.IsAdmin("U3");
            Assert.True(isAdminBeforeUpgrade);
            Assert.False(isAdminAfterUpgrade);
            _mockUserService.UpgradeToAdmin(userRequest);
        }
        
        [Fact]
        public void TestRevokeAdminOnNonAdmin()
        {
            var isAdminBeforeUpgrade = _mockUserService.IsAdmin("U1");
            var userRequest = new UserRequest {Id = "U1"};
            _mockUserService.RevokeAdmin(userRequest);
            var isAdminAfterUpgrade = _mockUserService.IsAdmin("U1");
            Assert.False(isAdminBeforeUpgrade);
            Assert.False(isAdminAfterUpgrade);
            _mockUserService.UpgradeToAdmin(userRequest);
        }
        
    }
}