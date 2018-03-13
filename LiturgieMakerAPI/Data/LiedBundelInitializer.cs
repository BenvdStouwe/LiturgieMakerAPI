using LiturgieMakerAPI.Liedbundels.Model;
using LiturgieMakerAPI.LiturgieMaker.Context;
using System.Collections.Generic;
using System.Linq;

namespace LiturgieMakerAPI.Data
{
    public static class LiedbundelInitializer
    {
        public static void Initialize(LiturgieMakerContext context)
        {
            if (context.Liedbundel.Any())
            {
                return;
            }

            var psalmboek = context.Add(BuildLiedbundel("Psalm", 150)).Entity;
            var opwekking = context.Add(BuildLiedbundel("Opwekking", 795));

            var lied = context.Add(BuildLied("Juich aarde", 4, 100, psalmboek)).Entity;

            var vers1 = context.Add(BuildVers(1, lied, @"Juich, aarde, juich alom den HEER;
Dient God met blijdschap, geeft Hem eer;
Komt, nadert voor Zijn aangezicht;
Zingt Hem een vrolijk lofgedicht.")).Entity;
            var vers2 = context.Add(BuildVers(2, lied, @"De HEER is God; erkent, dat Hij
Ons heeft gemaakt (en geenszins wij)
Tot schapen, die Hij voedt en weidt;
Een volk, tot Zijnen dienst bereid.")).Entity;
            var vers3 = context.Add(BuildVers(3, lied, @"Gaat tot Zijn poorten in met lof,
Met lofzang in Zijn heilig hof;
Looft Hem aldaar met hart en stem;
Prijst Zijnen naam, verheerlijkt Hem.")).Entity;
            var vers4 = context.Add(BuildVers(4, lied, @"Want goedertieren is de HEER;
Zijn goedheid eindigt nimmermeer;
Zijn trouw en waarheid houdt haar kracht
Tot in het laatste nageslacht.")).Entity;

            context.SaveChanges();
        }

        private static Liedbundel BuildLiedbundel(string naam, int aantalLiederen)
        {
            return new Liedbundel
            {
                Naam = naam,
                AantalLiederen = aantalLiederen,
                Liederen = new List<Lied>()
            };
        }

        private static Lied BuildLied(string naam, int aantalVerzen, int liednummer, Liedbundel liedbundel)
        {
            return new Lied
            {
                Naam = naam,
                AantalVerzen = aantalVerzen,
                LiedNummer = liednummer,
                Liedbundel = liedbundel
            };
        }

        private static Vers BuildVers(int versNummer, Lied lied, string tekst)
        {
            return new Vers
            {
                VersNummer = versNummer,
                Lied = lied,
                Tekst = tekst
            };
        }
    }
}