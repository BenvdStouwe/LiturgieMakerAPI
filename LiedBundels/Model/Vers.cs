using System.ComponentModel.DataAnnotations;

namespace LiturgieMakerAPI.Liedbundels.Model
{
    public class Vers
    {
        public long? Id { get; set; }
        public int VersNummer { get; set; }
        [Required]
        public string Tekst { get; set; }
        [Required]
        public Lied Lied { get; set; }
    }
}
