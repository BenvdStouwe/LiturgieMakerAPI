using System.Collections.Generic;

namespace LiturgieMakerAPI.LiedBundels.Model
{
    public class Lied
    {
        public long Id { get; set; }
        public int LiedNummer { get; set; }
        public string Naam { get; set; }
        public int AantalVerzen { get; set; }
        public IEnumerable<Vers> Verzen { get; set; }
        public LiedBundel LiedBundel { get; set; }
    }
}
