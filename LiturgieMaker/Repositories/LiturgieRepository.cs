using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Repositories
{
    public class LiturgieRepository
    {
        private readonly LiturgieContext _context;

        public LiturgieRepository(LiturgieContext context)
        {
            _context = context;

            if (_context.Liturgieen.Count() == 0)
            {
                var liturige = new Liturgie
                {
                    Titel = "Test liturgie",
                    Aanvangsdatum = new DateTime()
                };

                _context.Add(liturige);
            }
        }

        public Liturgie GetLiturgie(int id)
        {
            return _context.Liturgieen.FirstOrDefault(l => l.Id == id);
        }

        public IEnumerable<Liturgie> GetLiturgieen()
        {
            return _context.Liturgieen;
        }
    }
}