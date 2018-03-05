using System.Collections.Generic;
using System.ComponentModel;

namespace LiturgieMakerAPI.LiturgieMaker.Model
{
    public abstract class LiturgieItem
    {
        public int? Id { get; set; }
        public int Index { get; set; }
        public Liturgie Liturgie { get; set; }
        public virtual LiturgieItemSoort Soort { get; }
    }

    public enum LiturgieItemSoort
    {
        [Description("Lied")]
        LIED = 1,
        [Description("Schriftlezing")]
        SCHRIFTLEZING
    }
}
