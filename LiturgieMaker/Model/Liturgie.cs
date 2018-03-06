using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LiturgieMakerAPI.LiturgieMaker.Model
{
    public class Liturgie
    {
        public long? Id { get; set; }
        public string Titel { get; set; }
        public DateTime Aanvangsdatum { get; set; }
        public DateTime Publicatiedatum { get; set; }
        public IEnumerable<LiturgieItem> Items { get; set; }

        public void AddItem(LiturgieItem item)
        {
            Items.Append(item);
        }

        public void AddItems(IEnumerable<LiturgieItem> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }
    }

    public class LiturgieDto
    {
        public long? Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Titel { get; set; }
        [Required]
        public DateTime Aanvangsdatum { get; set; }
        [Required]
        public DateTime Publicatiedatum { get; set; }
        public IEnumerable<LiturgieItemDto> Items { get; set; }
    }
}
