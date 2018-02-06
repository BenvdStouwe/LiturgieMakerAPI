using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;

namespace LiturgieMakerAPI.LiturgieMaker.Repositories
{
    public class LiturgieRepository
    {
        private readonly LiturgieMakerContext _context;

        public LiturgieRepository(LiturgieMakerContext context)
        {
            _context = context;

            if (!_context.Liturgieen.Any())
            {
                var liturgie = new Liturgie
                {
                    Titel = "Test liturgie",
                    Aanvangsdatum = DateTime.Now,
                    Publicatiedatum = DateTime.Now
                };

                var items = new List<LiturgieItem> {
                    new Lied {
                        Index = 0,
                        Liturgie = liturgie
                    },
                    new Schriftlezing {
                        Index = 1,
                        Liturgie = liturgie,
                        Hoofdstuk = 5
                    },
                    new Lied {
                        Index = 2,
                        Liturgie = liturgie
                    }
                };

                liturgie.Items = items;

                _context.Liturgieen.Add(liturgie);
                _context.SaveChanges();
            }
        }

        public Liturgie GetLiturgie(int id)
        {
            return _context.Liturgieen.FirstOrDefault(l => l.LiturgieId == id);
        }

        public IEnumerable<Liturgie> GetLiturgieen()
        {
            return _context.Liturgieen.ToList();
        }
    }
}
