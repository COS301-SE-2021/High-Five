using src.Storage;

namespace src.Subsystems.User
{
    public class UserService: IUserService
    {
        private readonly IStorageManager _storageManager;
        
        public UserService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public bool IsAdmin()
        {
            return false;
        }
    }
}