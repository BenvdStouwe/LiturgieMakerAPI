using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LiturgieMakerAPI.Liedbundels.Model
{
    public class Lied
    {
        public long? Id { get; set; }
        public int LiedNummer { get; set; }
        public string Naam { get; set; }
        public int AantalVerzen { get; set; }
        public IEnumerable<Vers> Verzen { get; set; }
        [Required]
        public Liedbundel Liedbundel { get; set; }

        public void AddVers(Vers vers)
        {
            if (!Verzen.Any(v => v.Id == vers.Id))
            {
                Verzen.Append(vers);
            }
        }

        public void AddVerzen(IEnumerable<Vers> verzen)
        {
            foreach (var vers in verzen)
            {
                AddVers(vers);
            }
        }

        public void AddVerzen(params Vers[] verzen)
        {
            foreach (var vers in verzen)
            {
                AddVers(vers);
            }
        }
    }
}
