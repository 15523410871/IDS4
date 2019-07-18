using Microsoft.EntityFrameworkCore;

namespace Service.Entity
{
    public class IDSContext : DbContext
    {
        public IDSContext(DbContextOptions<IDSContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
    }
}
