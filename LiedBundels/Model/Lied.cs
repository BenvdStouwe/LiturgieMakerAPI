namespace LiturgieMakerAPI.LiedBundels.Model
{
    public class Lied
    {
        public long Id { get; set; }
        public string Naam { get; set; }
        public int AantalVerzen { get; set; }
        public LiedBundel LiedBundel { get; set; }
    }
}