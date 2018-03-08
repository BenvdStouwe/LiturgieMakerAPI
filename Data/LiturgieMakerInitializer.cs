using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.Liedbundels.Context;
using LiturgieMakerAPI.Liedbundels.Model;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;

namespace LiturgieMakerAPI.Data
{
    internal class LiturgieMakerInitializer
    {
        private readonly LiturgieMakerContext _context;
        private readonly LiedbundelsContext _liedbundelsContext;

        public LiturgieMakerInitializer(LiturgieMakerContext context, LiedbundelsContext liedbundelsContext)
        {
            _context = context;
            _liedbundelsContext = liedbundelsContext;
        }

        public void Initialize()
        {
            if (_context.Liturgieen.Any())
            {
                return;
            }

            var liturgie = NieuweLiturgie("Test liturgie", DateTime.Now, DateTime.Now.AddDays(-1));
            var liturgie2 = NieuweLiturgie("Nog een test liturgie", DateTime.Now, DateTime.Now.AddDays(2));

            var psalmboek = _liedbundelsContext.Liedbundels.FirstOrDefault(lb => lb.Naam == "Psalm");
            var opwekking = _liedbundelsContext.Liedbundels.FirstOrDefault(lb => lb.Naam == "Opwekking");

            var item1 = new LiedItem
            {
                Index = 0,
                Liturgie = liturgie,
                Lied = psalmboek.Liederen.FirstOrDefault(l => l.LiedNummer == 100)
            };

            var item2 = new SchriftlezingItem
            {
                Index = 1,
                Liturgie = liturgie,
                Hoofdstuk = 5
            };

            _context.Add(item1);
            _context.Add(item2);

            _context.SaveChanges();
        }

        private Liturgie NieuweLiturgie(string titel, DateTime aanvangsdatum, DateTime publicatieDatum)
        {
            return _context.Add(new Liturgie
            {
                Titel = titel,
                Aanvangsdatum = aanvangsdatum,
                Publicatiedatum = publicatieDatum
            }).Entity;
        }
    }
}
