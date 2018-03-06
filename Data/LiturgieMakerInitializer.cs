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
    internal static class LiturgieMakerInitializer
    {
        public static void Initialize(LiturgieMakerContext liturgieMakerContext, LiedbundelsContext liedbundelsContext, bool truncate = false)
        {
            if (liturgieMakerContext.Liturgieen.Any())
            {
                if (truncate)
                {
                    liturgieMakerContext.Liturgieen.ToList().ForEach(l => liturgieMakerContext.Remove(l));
                }
                else
                {
                    return;
                }
            }

            var liturgie = new Liturgie
            {
                Titel = "Test liturgie",
                Aanvangsdatum = DateTime.Now,
                Publicatiedatum = DateTime.Now
            };

            var psalmboek = liedbundelsContext.Liedbundels.FirstOrDefault(lb => lb.Naam == "Psalm");
            var opwekking = liedbundelsContext.Liedbundels.FirstOrDefault(lb => lb.Naam == "Opwekking");

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

            var liturgie2 = new Liturgie
            {
                Titel = "Nog een test liturgie",
                Aanvangsdatum = DateTime.Now,
                Publicatiedatum = DateTime.Now.AddDays(2)
            };
            liturgieMakerContext.Add(liturgie2);

            liturgieMakerContext.Add(liturgie);
            liturgieMakerContext.SaveChanges();
        }
    }
}
