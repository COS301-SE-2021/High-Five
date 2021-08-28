namespace src.Subsystems.Admin
{
    public interface IAdminValidator
    {
        public bool IsAdmin(string userId);
        public bool RevokeAdmin(string userId);
    }
}