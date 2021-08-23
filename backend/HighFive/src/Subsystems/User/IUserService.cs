using Org.OpenAPITools.Models;

namespace src.Subsystems.User
{
    public interface IUserService
    {
        public GetAllUsersResponse GetAllUsers();
        public void DeleteMedia(UserRequest request);
        public void DeleteOwnMedia();
        public void DeleteUser(UserRequest request);
        public void UpgradeToAdmin(UserRequest request);
    }
}