using System.Collections.Generic;

namespace LiturgieMakerAPI.Liedbundels.Model
{
    public class Liedbundel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public int AantalLiederen { get; set; }
        public IEnumerable<Lied> Liederen { get; set; }
    }
}
