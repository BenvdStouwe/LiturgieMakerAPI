using Microsoft.EntityFrameworkCore;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;

namespace LiturgieMakerAPI.LiturgieMaker.Context
{
    public class LiturgieMakerContext : DbContext
    {
        public LiturgieMakerContext(DbContextOptions<LiturgieMakerContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // LiturgieItems
            builder.Entity<Schriftlezing>();
            builder.Entity<Lied>();

            base.OnModelCreating(builder);
        }

        public DbSet<Liturgie> Liturgieen { get; set; }
    }
}
