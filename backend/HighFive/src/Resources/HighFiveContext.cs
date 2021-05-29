using Microsoft.EntityFrameworkCore;
using src.Subsystems.User.Data;

namespace src.Resources
{
    public class HighFiveContext : DbContext
    {
        public HighFiveContext(DbContextOptions<HighFiveContext> options): base(options) 
        { }
        
        private DbSet<User> Users { get; set; }
        
    }
}