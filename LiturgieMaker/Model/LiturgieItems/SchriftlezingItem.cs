namespace LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems
{
    public class SchriftlezingItem : LiturgieItem
    {
        public override LiturgieItemSoort Soort => LiturgieItemSoort.SCHRIFTLEZING;
        public int Hoofdstuk { get; set; }
    }

    public class SchriftlezingItemDto : LiturgieItemDto
    {
        public int Hoofdstuk { get; set; }
    }
}
