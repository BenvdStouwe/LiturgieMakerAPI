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

        public LiturgieRepository(LiturgieMakerContext context)
        {
            _context = context;
        }

        public virtual Liturgie GetLiturgie(long id)
        {
            return _context.Liturgie
                .Where(l => !l.Deleted)
                .Include(l => l.Items)
                .SingleOrDefault(l => l.Id == id);
        }

        public IEnumerable<Liturgie> GetLiturgieen()
        {
            return _context.Liturgie
                .Where(l => !l.Deleted)
                .ToList();
        }

        public Liturgie SaveLiturgie(Liturgie liturgie)
        {
            if (liturgie.Id != null)
            {
                _context.Update(liturgie);
            }
            else
            {
                _context.Add(liturgie);
            }
            _context.SaveChanges();
            return liturgie;
        }

        public virtual void DeleteLiturgie(Liturgie liturgie)
        {
            liturgie.Deleted = true;
            SaveLiturgie(liturgie);
        }
    }
}
