using Org.OpenAPITools.Models;

namespace src.Subsystems.User
{
    public interface IUserService
    {
        public GetAllUsersResponse GetAllUsers();
        public void DeleteMedia(UserRequest request);
        public void DeleteUser(UserRequest request);
        public bool UpgradeToAdmin(UserRequest request);
    }
}