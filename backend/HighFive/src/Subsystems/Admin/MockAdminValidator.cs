using System.Collections.Generic;
using Moq;

namespace src.Subsystems.Admin
{
    public class MockAdminValidator: IAdminValidator
    {
        public List<string> AdminUsers;

        public MockAdminValidator()
        {
            AdminUsers = new List<string> {"U3"};
        }
        
        public bool IsAdmin(string userId)
        {
            return AdminUsers.Contains(userId);
        }

        public bool RevokeAdmin(string userId)
        {
            return AdminUsers.Remove(userId);
        }

        public bool UpgradeToAdmin(string userId)
        {
            AdminUsers.Add(userId);
            return true;
        }
    }
}