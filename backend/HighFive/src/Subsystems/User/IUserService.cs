using System.Threading.Tasks;
using Org.OpenAPITools.Models;

namespace src.Subsystems.User
{
    public interface IUserService
    {
        public GetAllUsersResponse GetAllUsers();
        public Task DeleteMedia(UserRequest request);
        public void DeleteUser(UserRequest request);
        public bool UpgradeToAdmin(UserRequest request);
        public bool IsAdmin(string userId);
    }
}