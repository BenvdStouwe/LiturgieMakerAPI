using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiturgieMakerAPI.LiturgieMaker.Model
{
    public abstract class LiturgieItem
    {
        public int LiturgieItemId { get; set; }
        public int Index { get; set; }
        public Liturgie Liturgie { get; set; }
        public virtual string Soort { get; }
    }
}
