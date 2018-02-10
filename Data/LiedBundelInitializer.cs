using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.Liedbundels.Context;
using LiturgieMakerAPI.Liedbundels.Model;

namespace LiturgieMakerAPI.Data
{
    public static class LiedbundelInitializer
    {
        public static void Initialize(LiedbundelsContext context, bool truncate = false)
        {
            if (context.Liedbundels.Any())
            {
                if (truncate)
                {
                    context.Liedbundels.ToList().ForEach(lb => context.Remove(lb));
                }
                else
                {
                    return;
                }
            }

            var psalmboek = new Liedbundel
            {
                Naam = "Psalm",
                AantalLiederen = 150,
                Liederen = new List<Lied>()
            };
            var opwekking = new Liedbundel
            {
                Naam = "Opwekking",
                AantalLiederen = 795,
                Liederen = new List<Lied>()
            };
            context.Add(psalmboek);
            context.Add(opwekking);

            var lied = new Lied
            {
                Naam = "Juich aarde",
                AantalVerzen = 4,
                LiedNummer = 100,
                Liedbundel = psalmboek
            };
            context.Add(lied);

            var vers1 = new Vers
            {
                VersNummer = 1,
                Lied = lied,
                Tekst = @"Juich, aarde, juich alom den HEER;
Dient God met blijdschap, geeft Hem eer;
Komt, nadert voor Zijn aangezicht;
Zingt Hem een vrolijk lofgedicht."
            };
            var vers2 = new Vers
            {
                VersNummer = 2,
                Lied = lied,
                Tekst = @"De HEER is God; erkent, dat Hij
Ons heeft gemaakt (en geenszins wij)
Tot schapen, die Hij voedt en weidt;
Een volk, tot Zijnen dienst bereid."
            };
            var vers3 = new Vers
            {
                VersNummer = 3,
                Lied = lied,
                Tekst = @"Gaat tot Zijn poorten in met lof,
Met lofzang in Zijn heilig hof;
Looft Hem aldaar met hart en stem;
Prijst Zijnen naam, verheerlijkt Hem."
            };
            var vers4 = new Vers
            {
                VersNummer = 4,
                Lied = lied,
                Tekst = @"Want goedertieren is de HEER;
Zijn goedheid eindigt nimmermeer;
Zijn trouw en waarheid houdt haar kracht
Tot in het laatste nageslacht."
            };
            context.Add(vers1);
            context.Add(vers2);
            context.Add(vers3);
            context.Add(vers4);
            var verzen = new List<Vers>
            {
                vers1, vers2, vers3, vers4
            };
            lied.Verzen = verzen;

            psalmboek.Liederen.Append(lied);

            context.SaveChanges();
        }
    }
}