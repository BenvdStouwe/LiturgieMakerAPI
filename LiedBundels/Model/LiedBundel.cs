using System.Collections.Generic;
using System.Linq;

namespace LiturgieMakerAPI.LiedBundels.Model
{
    public class LiedBundel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public int AantalLiederen => Liederen.Count();
        public IEnumerable<Lied> Liederen { get; set; }
    }
}
