using LiturgieMakerAPI.LiedBundels.Model;

namespace LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems
{
    public class LiedItem : LiturgieItem
    {
        public override string Soort => "Lied";
        public Lied Lied { get; set; }
    }
}