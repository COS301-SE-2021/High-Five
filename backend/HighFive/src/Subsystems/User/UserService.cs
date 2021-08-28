﻿using System;
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
            var response = false;
            var oldContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var adminsFile = _storageManager.GetFile("admins.txt", "").Result;
            _storageManager.SetBaseContainer(oldContainer);
            var adminsArray = adminsFile.ToText().Result.Split("\n", StringSplitOptions.None);
            //the above line splits the text file's contents by newlines into an array
            var adminListString = string.Empty;
            foreach (var admin in adminsArray)
            {
                adminListString += admin;
                if (!adminsArray[^1].Equals(admin))
                {
                    adminListString += "\n";
                }
            }
            if (adminsArray.IndexOf(request.Id) == -1)
            {
                response = true;
                adminListString += "\n" +request.Id;
            }

            adminsFile.UploadText(adminListString);
            return response;
        }

        public bool IsAdmin(string userId)
        {
            return _adminValidator.IsAdmin(userId);
        }

        public bool RevokeAdmin(UserRequest request)
        {
            return _adminValidator.RevokeAdmin(request.Id);
        }
    }
}