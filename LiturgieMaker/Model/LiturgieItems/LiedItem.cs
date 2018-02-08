using LiturgieMakerAPI.Liedbundels.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems
{
    public class LiedItem : LiturgieItem
    {
        public override LiturgieItemSoort Soort => LiturgieItemSoort.LIED;
        public int LiedNummer { get; set; }
        public Lied Lied { get; set; }
    }
}
