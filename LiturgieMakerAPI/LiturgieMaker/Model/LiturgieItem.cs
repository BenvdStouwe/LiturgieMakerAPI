using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LiturgieMakerAPI.LiturgieMaker.Model
{
    public abstract class LiturgieItem
    {
        public int? Id { get; set; }
        public int Index { get; set; }
        public Liturgie Liturgie { get; set; }
        public virtual LiturgieItemSoort Soort { get; }
    }

    public abstract class LiturgieItemDto
    {
        public long? Id { get; set; }
        [Required]
        public int Index { get; set; }
        [Required]
        [EnumDataType(typeof(LiturgieItemSoort))]
        public LiturgieItemSoort Soort { get; set; }
    }

    public enum LiturgieItemSoort
    {
        [Description("Lied")]
        LIED = 1,
        [Description("Schriftlezing")]
        SCHRIFTLEZING
    }
}
