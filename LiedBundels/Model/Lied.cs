using System.Collections.Generic;

namespace LiturgieMakerAPI.Liedbundels.Model
{
    public class Lied
    {
        public long Id { get; set; }
        public int LiedNummer { get; set; }
        public string Naam { get; set; }
        public int AantalVerzen { get; set; }
        public IEnumerable<Vers> Verzen { get; set; }
        public Liedbundel Liedbundel { get; set; }
    }
}
