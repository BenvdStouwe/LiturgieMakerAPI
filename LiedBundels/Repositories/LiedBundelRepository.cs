using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.Data;
using LiturgieMakerAPI.Liedbundels.Context;
using LiturgieMakerAPI.Liedbundels.Model;
using Microsoft.EntityFrameworkCore;

namespace LiturgieMakerAPI.Liedbundels.Repositories
{
    public class LiedbundelRepository
    {
        private readonly LiedbundelsContext _context;
        public LiedbundelRepository(LiedbundelsContext context)
        {
            _context = context;
        }

        public Liedbundel GetLiedbundel(long id)
        {
            return _context.Liedbundels
                .Include(lb => lb.Liederen)
                .SingleOrDefault(lb => lb.Id == id);
        }

        public IEnumerable<Liedbundel> SearchLiedbundel(string naam)
        {
            return _context.Liedbundels
                .Where(lb => lb.Naam.IndexOf(naam, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public IEnumerable<Liedbundel> GetLiedbundels()
        {
            return _context.Liedbundels
                .ToList();
        }
    }
}
