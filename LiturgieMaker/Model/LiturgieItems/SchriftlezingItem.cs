namespace LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems
{
    public class SchriftlezingItem : LiturgieItem
    {
        public override string Soort => "Schriftlezing";
        public int Hoofdstuk { get; set; }
    }
}
