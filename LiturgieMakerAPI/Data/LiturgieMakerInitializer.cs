using LiturgieMakerAPI.Liedbundels.Model;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LiturgieMakerAPI.Data
{
    public static class LiturgieMakerInitializer
    {
        public static void Initialize(LiturgieMakerContext context)
        {
            if (context.Database.IsMySql())
            {
                context.Database.Migrate();
            }

            if (context.Liturgie.Any())
            {
                return;
            }

            if (!context.Liedbundel.Any())
            {
                LiedbundelInitializer.Initialize(context);
            }

            var psalmboek = context.Liedbundel.FirstOrDefault(lb => lb.Naam == "Psalm");
            var opwekking = context.Liedbundel.FirstOrDefault(lb => lb.Naam == "Opwekking");

            var liturgie = context.Add(BuildLiturgie("Test liturgie", DateTime.Now, DateTime.Now.AddDays(-1))).Entity;
            var liturgie2 = context.Add(BuildLiturgie("Nog een test liturgie", DateTime.Now, DateTime.Now.AddDays(2))).Entity;

            var item1 = context.Add(BuildLiedItem(liturgie, 0, psalmboek.Liederen.SingleOrDefault(l => l.LiedNummer == 100))).Entity;
            var item2 = context.Add(BuildSchriftlezingItem(liturgie, 1, 5)).Entity;

            context.SaveChanges();
        }

        public static Liturgie BuildLiturgie(string titel, DateTime aanvangsdatum, DateTime publicatieDatum, bool deleted = false)
        {
            return new Liturgie
            {
                Titel = titel,
                Aanvangsdatum = aanvangsdatum,
                Publicatiedatum = publicatieDatum,
                Deleted = deleted
            };
        }

        public static LiedItem BuildLiedItem(Liturgie liturgie, int index, Lied lied)
        {
            return new LiedItem
            {
                Liturgie = liturgie,
                Index = index,
                Lied = lied
            };
        }

        public static SchriftlezingItem BuildSchriftlezingItem(Liturgie liturgie, int index, int hoofdstuk)
        {
            return new SchriftlezingItem
            {
                Liturgie = liturgie,
                Index = index,
                Hoofdstuk = hoofdstuk
            };
        }
    }
}
