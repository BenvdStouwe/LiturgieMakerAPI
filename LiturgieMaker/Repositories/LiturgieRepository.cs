using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Repositories
{
    public class LiturgieRepository
    {
        private readonly LiturgieMakerContext _context;

        public LiturgieRepository(LiturgieMakerContext context)
        {
            _context = context;

            if (_context.Liturgieen.Count() == 0)
            {
                var liturige = new Liturgie
                {
                    Titel = "Test liturgie",
                    Aanvangsdatum = DateTime.Now,
                    Publicatiedatum = DateTime.Now
                };

                _context.Liturgieen.Add(liturige);
                _context.SaveChanges();
            }
        }

        public Liturgie GetLiturgie(int id)
        {
            return _context.Liturgieen.FirstOrDefault(l => l.Id == id);
        }

        public IEnumerable<Liturgie> GetLiturgieen()
        {
            return _context.Liturgieen.ToList();
        }
    }
}
