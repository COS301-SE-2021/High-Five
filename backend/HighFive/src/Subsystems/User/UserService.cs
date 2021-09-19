using System;
using System.Threading.Tasks;
using Accord.Math;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.Admin;

namespace src.Subsystems.User
{
    public class UserService: IUserService
    {
        private readonly IStorageManager _storageManager;
        private readonly IAdminValidator _adminValidator;

        public UserService(IStorageManager storageManager, IAdminValidator adminValidator)
        {
            _storageManager = storageManager;
            _adminValidator = adminValidator;
        }

        public GetAllUsersResponse GetAllUsers()
        {
            var list = _storageManager.GetAllUsers().Result;
            return new GetAllUsersResponse{Users = list};
        }

        public async Task DeleteMedia(UserRequest request)
        {
            await _storageManager.DeleteAllFilesInContainer(request.Id);
        }

        public bool UpgradeToAdmin(UserRequest request)
        {
            return _adminValidator.UpgradeToAdmin(request.Id);
        }

        public bool IsAdmin(string userId)
        {
            return _adminValidator.IsAdmin(userId);
        }

        public bool RevokeAdmin(UserRequest request)
        {
            return _adminValidator.RevokeAdmin(request.Id);
        }

        public void StoreUserInfo(string id, string displayName, string email)
        {
            _storageManager.StoreUserInfo(id, displayName, email);
        }

        public bool SetBaseContainer(string containerName)
        {
            /*
             *      Description:
             * This function tests if a baseContainer has been set yet, it will be called before any of the
             * other StorageManager method code executes. If a base container has already been set, this code
             * will do nothing, else it will set the base container to the user's Azure AD B2C unique object
             * id - hence pointing towards the user's own container within the storage.
             *
             *      Parameters:
             * -> containerName: the user's id that will be used as the container name.
             */
            
            if (!_storageManager.IsContainerSet())
            {
                return _storageManager.SetBaseContainer(containerName).Result;
            }
            return true;
        }
    }
}