using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.Liedbundels.Context;
using LiturgieMakerAPI.Liedbundels.Model;

namespace LiturgieMakerAPI.Data
{
    internal class LiedbundelInitializer
    {
        private readonly LiedbundelsContext _context;

        public LiedbundelInitializer(LiedbundelsContext context)
        {
            _context = context;
        }

        public void Initialize(bool truncate = false)
        {
            if (_context.Liedbundels.Any())
            {
                if (truncate)
                {
                    _context.Liedbundels.ToList().ForEach(lb => _context.Remove(lb));
                }
                else
                {
                    return;
                }
            }

            var psalmboek = NieuweLiedbundel("Psalm", 150);
            var opwekking = NieuweLiedbundel("Opwekking", 795);

            var lied = NieuwLied("Juich aarde", 4, 100, psalmboek);

            var vers1 = NieuwVers(1, lied, @"Juich, aarde, juich alom den HEER;
Dient God met blijdschap, geeft Hem eer;
Komt, nadert voor Zijn aangezicht;
Zingt Hem een vrolijk lofgedicht.");
            var vers2 = NieuwVers(2, lied, @"De HEER is God; erkent, dat Hij
Ons heeft gemaakt (en geenszins wij)
Tot schapen, die Hij voedt en weidt;
Een volk, tot Zijnen dienst bereid.");
            var vers3 = NieuwVers(3, lied, @"Gaat tot Zijn poorten in met lof,
Met lofzang in Zijn heilig hof;
Looft Hem aldaar met hart en stem;
Prijst Zijnen naam, verheerlijkt Hem.");
            var vers4 = NieuwVers(4, lied, @"Want goedertieren is de HEER;
Zijn goedheid eindigt nimmermeer;
Zijn trouw en waarheid houdt haar kracht
Tot in het laatste nageslacht.");

            _context.SaveChanges();
        }

        private Liedbundel NieuweLiedbundel(string naam, int aantalLiederen)
        {
            return _context.Add(new Liedbundel
            {
                Naam = naam,
                AantalLiederen = aantalLiederen,
                Liederen = new List<Lied>()
            }).Entity;
        }

        private Lied NieuwLied(string naam, int aantalVerzen, int liednummer, Liedbundel liedbundel)
        {
            return _context.Add(new Lied
            {
                Naam = naam,
                AantalVerzen = aantalVerzen,
                LiedNummer = liednummer,
                Liedbundel = liedbundel
            }).Entity;
        }

        private Vers NieuwVers(int versNummer, Lied lied, string tekst)
        {
            return _context.Add(new Vers
            {
                VersNummer = versNummer,
                Lied = lied,
                Tekst = tekst
            }).Entity;
        }
    }
}