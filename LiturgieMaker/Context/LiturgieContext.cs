using Microsoft.EntityFrameworkCore;
using LiturgieMakerAPI.LiturgieMaker.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Context
{
    public class LiturgieContext : DbContext
    {
        public LiturgieContext(DbContextOptions<LiturgieContext> options)
            : base(options) { }

        public DbSet<Liturgie> Liturgieen { get; set; }
        public DbSet<LiturgieItem> LiturgieItems { get; set; }
    }
}