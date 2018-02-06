using System.Collections.Generic;

namespace LiturgieMakerAPI.LiturgieMaker.Model
{
    public abstract class LiturgieItem
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public Liturgie Liturgie { get; set; }
        public virtual string Soort { get; }
    }
}
