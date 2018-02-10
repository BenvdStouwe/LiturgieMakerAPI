using LiturgieMakerAPI.Liedbundels.Model;
using Microsoft.EntityFrameworkCore;

namespace LiturgieMakerAPI.Liedbundels.Context
{
    public class LiedbundelsContext : DbContext
    {
        public LiedbundelsContext(DbContextOptions<LiedbundelsContext> options)
            : base(options) { }

        public DbSet<Liedbundel> Liedbundels { get; set; }
        public DbSet<Lied> Liederen { get; set; }
    }
}
