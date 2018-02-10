namespace LiturgieMakerAPI.Liedbundels.Model
{
    public class Vers
    {
        public long Id { get; set; }
        public int VersNummer { get; set; }
        public string Tekst { get; set; }
        public Lied Lied { get; set; }
    }
}
