using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems
{
    public class Schriftlezing : LiturgieItem
    {
        public override string Soort => "Schriftlezing";
        public int Hoofdstuk { get; set; }
        // public List<int> Verzen { get; set; }
    }
}
