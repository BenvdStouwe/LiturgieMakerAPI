using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.Liedbundels.Model;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;

namespace LiturgieMakerAPI.Data
{
    internal class LiturgieMakerInitializer
    {
        private readonly LiturgieMakerContext _context;

        public LiturgieMakerInitializer(LiturgieMakerContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            if (_context.Liturgieen.Any())
            {
                return;
            }

            if (!_context.Liedbundels.Any())
            {
                new LiedbundelInitializer(_context).Initialize();
            }

            var psalmboek = _context.Liedbundels.FirstOrDefault(lb => lb.Naam == "Psalm");
            var opwekking = _context.Liedbundels.FirstOrDefault(lb => lb.Naam == "Opwekking");

            var liturgie = NieuweLiturgie("Test liturgie", DateTime.Now, DateTime.Now.AddDays(-1));
            var liturgie2 = NieuweLiturgie("Nog een test liturgie", DateTime.Now, DateTime.Now.AddDays(2));

            var item1 = NieuwLiedItem(liturgie, 0, psalmboek.Liederen.SingleOrDefault(l => l.LiedNummer == 100));
            var item2 = NieuwSchriftlezingItem(liturgie, 1, 5);

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

        private LiedItem NieuwLiedItem(Liturgie liturgie, int index, Lied lied)
        {
            return _context.Add(new LiedItem
            {
                Liturgie = liturgie,
                Index = index,
                Lied = lied
            }).Entity;
        }

        private SchriftlezingItem NieuwSchriftlezingItem(Liturgie liturgie, int index, int hoofdstuk)
        {
            return _context.Add(new SchriftlezingItem
            {
                Liturgie = liturgie,
                Index = index,
                Hoofdstuk = hoofdstuk
            }).Entity;
        }
    }
}
