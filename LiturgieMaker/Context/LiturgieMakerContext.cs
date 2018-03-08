using Microsoft.EntityFrameworkCore;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;
using LiturgieMakerAPI.Liedbundels.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Context
{
    public class LiturgieMakerContext : DbContext
    {
        public LiturgieMakerContext(DbContextOptions<LiturgieMakerContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // LiturgieItems
            builder.Entity<SchriftlezingItem>();
            builder.Entity<LiedItem>();

            base.OnModelCreating(builder);
        }

        public DbSet<Liturgie> Liturgieen { get; set; }

        // Liedbundels
        public DbSet<Liedbundel> Liedbundels { get; set; }
        public DbSet<Lied> Liederen { get; set; }
    }
}
