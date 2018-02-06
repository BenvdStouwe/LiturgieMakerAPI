using Microsoft.EntityFrameworkCore;
using LiturgieMakerAPI.LiturgieMaker.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Context
{
    public class LiturgieMakerContext : DbContext
    {
        public LiturgieMakerContext(DbContextOptions<LiturgieMakerContext> options)
            : base(options) { }

        public DbSet<Liturgie> Liturgieen { get; set; }
        public DbSet<LiturgieItem> LiturgieItems { get; set; }
    }
}
