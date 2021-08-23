using System;
using Org.OpenAPITools.Models;
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

        public GetAllUsersResponse GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public void DeleteMedia(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public void DeleteOwnMedia()
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public void UpgradeToAdmin(UserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}