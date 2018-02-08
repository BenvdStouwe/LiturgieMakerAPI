using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.Liedbundels.Model;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;
using Microsoft.EntityFrameworkCore;

namespace LiturgieMakerAPI.LiturgieMaker.Repositories
{
    public class LiturgieRepository
    {
        private readonly LiturgieMakerContext _context;

        public LiturgieRepository(LiturgieMakerContext context)
        {
            _context = context;
        }

        public Liturgie GetLiturgie(long id)
        {
            return _context.Liturgieen
                .Include(l => l.Items)
                .SingleOrDefault(l => l.Id == id);
        }

        public IEnumerable<Liturgie> GetLiturgieen()
        {
            return _context.Liturgieen
                .ToList();
        }
    }
}
