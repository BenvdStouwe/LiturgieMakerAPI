using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LiturgieMakerAPI.LiturgieMaker.Repositories
{
    public class LiturgieRepository
    {
        private readonly LiturgieMakerContext _context;

        public LiturgieRepository(LiturgieMakerContext context) => _context = context;

        public virtual Liturgie GetLiturgie(long id) => _context.Liturgie
            .WhereActief()
            .Include(l => l.Items)
            .SingleOrDefault(l => l.Id.Value == id);

        public virtual IEnumerable<Liturgie> GetLiturgieen(int page, int results) => _context.Liturgie
            .WhereActief()
            .OrderByDescending(l => l.Aanvangsdatum)
            .Skip((page - 1) * results)
            .Take(results)
            .ToList();

        public virtual int GetAantalLitugieen() => _context.Liturgie
            .WhereActief()
            .Count();

        public Liturgie SaveLiturgie(Liturgie liturgie)
        {
            var entry = liturgie.Id == null ? _context.Add(liturgie) : _context.Update(liturgie);
            _context.SaveChanges();
            return entry.Entity;
        }

        public virtual void DeleteLiturgie(Liturgie liturgie)
        {
            liturgie.Deleted = true;
            SaveLiturgie(liturgie);
        }
    }
}
