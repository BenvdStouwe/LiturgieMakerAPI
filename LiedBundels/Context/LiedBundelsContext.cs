using LiturgieMakerAPI.LiedBundels.Model;
using Microsoft.EntityFrameworkCore;

namespace LiturgieMakerAPI.LiedBundels.Context
{
    public class LiedBundelsContext : DbContext
    {
        public LiedBundelsContext(DbContextOptions<LiedBundelsContext> options)
            : base(options) { }

        public DbSet<LiedBundel> LiedBundels { get; set; }
        public DbSet<Lied> Liederen { get; set; }
    }
}
