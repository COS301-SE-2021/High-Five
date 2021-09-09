using System;
using System.Linq;
using Accord.Math;
using src.Storage;

namespace src.Subsystems.Admin
{
    public class AdminValidator: IAdminValidator
    {
        private readonly IStorageManager _storageManager;
        
        public AdminValidator(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public bool IsAdmin(string userId)
        {
            var oldContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var adminsFile = _storageManager.GetFile("admins.txt", "").Result;
            _storageManager.SetBaseContainer(oldContainer);
            var adminsArray = adminsFile.ToText().Result.Split("\n");
            //the above line splits the text file's contents by newlines into an array
            
            return adminsArray.IndexOf(userId) != -1;
        }

        public bool RevokeAdmin(string userId)
        {
            var oldContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var adminsFile = _storageManager.GetFile("admins.txt", "").Result;
            _storageManager.SetBaseContainer(oldContainer);
            var adminsList = adminsFile.ToText().Result.Split("\n").ToList();
            //the above line splits the text file's contents by newlines into an array
            var response = adminsList.Remove(userId);
            var updatedAdminList = string.Empty;
            foreach (var admin in adminsList)
            {
                updatedAdminList += admin;
                if (admin != adminsList[^1])
                {
                    updatedAdminList += "\n";
                }
            }

            adminsFile.UploadText(updatedAdminList);

            return response;
        }
    }
}