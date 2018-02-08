using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.Data;
using LiturgieMakerAPI.LiedBundels.Context;
using LiturgieMakerAPI.LiedBundels.Model;
using Microsoft.EntityFrameworkCore;

namespace LiturgieMakerAPI.LiedBundels.Repositories
{
    public class LiedBundelRepository
    {
        private readonly LiedBundelsContext _context;
        public LiedBundelRepository(LiedBundelsContext context)
        {
            _context = context;
        }

        public LiedBundel GetLiedBundel(long id)
        {
            return _context.LiedBundels
                .Include(lb => lb.Liederen)
                .SingleOrDefault(lb => lb.Id == id);
        }

        public LiedBundel GetLiedBundel(string naam)
        {
            return _context.LiedBundels
                .Include(lb => lb.Liederen)
                .FirstOrDefault(lb => lb.Naam == naam);
        }

        public IEnumerable<LiedBundel> GetLiedBundels()
        {
            return _context.LiedBundels
                .ToList();
        }
    }
}