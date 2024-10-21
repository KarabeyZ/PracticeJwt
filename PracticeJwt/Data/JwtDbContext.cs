using Microsoft.EntityFrameworkCore;
using PracticeJwt.Models;

namespace PracticeJwt.Data
{
    public class JwtDbContext : DbContext
    {
        public JwtDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
    }
}
