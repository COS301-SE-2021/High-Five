using Microsoft.EntityFrameworkCore;
using src.Subsystems.User.Data;

namespace src.Resources
{
    public class HighFiveContext : DbContext
    {
        public HighFiveContext(DbContextOptions<HighFiveContext> options): base(options) 
        { }
        
        public DbSet<User> Users { get; set; }
        
    }
}