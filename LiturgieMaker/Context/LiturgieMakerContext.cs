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
            builder.Entity<Liturgie>();
            builder.Entity<SchriftlezingItem>();
            builder.Entity<LiedItem>();

            builder.Entity<Liedbundel>();
            builder.Entity<Lied>();
            builder.Entity<Vers>();

            base.OnModelCreating(builder);
        }

        public DbSet<Liturgie> Liturgie { get; set; }

        // Liedbundels
        public DbSet<Liedbundel> Liedbundel { get; set; }
        public DbSet<Lied> Lied { get; set; }
    }
}
