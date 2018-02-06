using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.LiedBundels.Context;
using LiturgieMakerAPI.LiedBundels.Model;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;

namespace LiturgieMakerAPI.Data
{
    public static class LiturgieMakerInitializer
    {
        public static void Initialize(LiturgieMakerContext liturgieMakerContext, LiedBundelsContext liedBundelsContext)
        {
            if (liturgieMakerContext.Liturgieen.Any())
            {
                return;
            }

            var liturgie = new Liturgie
            {
                Titel = "Test liturgie",
                Aanvangsdatum = DateTime.Now,
                Publicatiedatum = DateTime.Now
            };

            var psalmboek = liedBundelsContext.LiedBundels.FirstOrDefault(lb => lb.Naam == "Psalm");
            var opwekking = liedBundelsContext.LiedBundels.FirstOrDefault(lb => lb.Naam == "Opwekking");

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

            liturgieMakerContext.Add(item1);
            liturgieMakerContext.Add(item2);

            var items = new List<LiturgieItem> {
                item1, item2
            };

            liturgie.Items = items;

            liturgieMakerContext.Liturgieen.Add(liturgie);
            liturgieMakerContext.SaveChanges();
        }
    }
}
