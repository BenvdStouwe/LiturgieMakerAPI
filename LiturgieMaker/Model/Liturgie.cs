using System;
using System.Collections.Generic;

namespace LiturgieMakerAPI.LiturgieMaker.Model
{
    public class Liturgie
    {
        public long Id { get; set; }
        public string Titel { get; set; }
        public DateTime Aanvangsdatum { get; set; }
        public DateTime Publicatiedatum { get; set; }
        public int AantalItems { get; set; }
        public IEnumerable<LiturgieItem> Items { get; set; }
    }
}
