using LiturgieMakerAPI.Liedbundels.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems
{
    public class LiedItem : LiturgieItem
    {
        public override string Soort => "Lied";
        public int LiedNummer { get; set; }
        public Lied Lied { get; set; }
    }
}
