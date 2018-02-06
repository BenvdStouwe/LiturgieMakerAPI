using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiturgieMakerAPI.LiturgieMaker.Model
{
    public class Liturgie
    {
        public long LiturgieId { get; set; }
        public string Titel { get; set; }
        public DateTime Aanvangsdatum { get; set; }
        public DateTime Publicatiedatum { get; set; }
        public IEnumerable<LiturgieItem> Items { get; set; }
    }
}
