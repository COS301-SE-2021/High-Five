using System.Threading.Tasks;
using Org.OpenAPITools.Models;

namespace src.Subsystems.User
{
    public interface IUserService
    {
        public GetAllUsersResponse GetAllUsers();
        public Task DeleteMedia(UserRequest request);
        public bool UpgradeToAdmin(UserRequest request);
        public bool IsAdmin(string userId);
        public bool RevokeAdmin(UserRequest request);
        public void StoreUserInfo(string id, string displayName, string email);
        public bool SetBaseContainer(string containerName);
    }
}