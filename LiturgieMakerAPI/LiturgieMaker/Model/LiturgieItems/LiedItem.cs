using LiturgieMakerAPI.Liedbundels.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems
{
    public class LiedItem : LiturgieItem
    {
        public override LiturgieItemSoort Soort => LiturgieItemSoort.LIED;
        public Lied Lied { get; set; }
    }

    public class LiedItemDto : LiturgieItemDto
    {
        public Lied Lied { get; set; }
    }
}
