using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LiturgieMakerAPI.LiturgieMaker.Controllers
{
    [Route("api/[controller]")]
    public class LiturgieController : Controller
    {
        public static readonly string ERROR_GEEN_VALIDE_LITURGIEITEMSOORT = "Eén of meer van de opgestuurde liturgie items had een onbekend type.";
        private LiturgieRepository _liturgieRepository;

        public LiturgieController(LiturgieRepository liturgieRepository)
        {
            _liturgieRepository = liturgieRepository;
        }

        /// <summary>
        /// Alle liturgieen ophalen
        /// </summary>
        /// <remarks>
        /// Haalt nog niet de teksten op
        /// </remarks>
        /// <returns>Alle liturgieen</returns>
        /// <response code="200">Returns the newly-created item</response>
        [HttpGet]
        public IActionResult Get()
        {
            var liturgieen = _liturgieRepository.GetLiturgieen();
            return Ok(liturgieen.Select(l => new LiturgieDto(l)));
        }

        /// <summary>
        /// Een enkele liturgie ophalen
        /// </summary>
        /// <remarks>
        /// Haalt ook items en teksten op
        /// </remarks>
        /// <param name="id">Uniek ID</param>
        /// <returns>Een liturgie</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LiturgieDto), 200)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public IActionResult Get(long id)
        {
            var liturgie = _liturgieRepository.GetLiturgie(id);

            if (liturgie == null)
            {
                return NotFound("Deze liturgie bestaat niet.");
            }

            return Ok(new LiturgieDto(liturgie));
        }

        /// <summary>
        /// Maak een nieuwe liturgie aan
        /// </summary>
        /// <param name="liturgieDto"></param>
        /// <returns>Liturgie</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LiturgieDto), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Post([FromForm]LiturgieDto liturgieDto)
        {
            var liturgie = FromDto(liturgieDto);

            if (liturgieDto.Items != null && liturgie.Items.Count(i => i != null) != liturgieDto.Items.Count())
            {
                return BadRequest(ERROR_GEEN_VALIDE_LITURGIEITEMSOORT);
            }

            _liturgieRepository.SaveLiturgie(liturgie);
            return CreatedAtAction("Get", new { id = liturgie.Id }, new LiturgieDto(liturgie));
        }

        private Liturgie FromDto(LiturgieDto liturgieDto)
        {
            var liturgie = new Liturgie
            {
                Titel = liturgieDto.Titel,
                Aanvangsdatum = liturgieDto.Aanvangsdatum,
                Publicatiedatum = liturgieDto.Publicatiedatum,
                Items = liturgieDto.Items?.Select(FromDto) ?? null
            };

            return liturgie;
        }

        private LiturgieItem FromDto(LiturgieItemDto itemDto)
        {
            if (!Enum.IsDefined(typeof(LiturgieItemSoort), itemDto.Soort))
            {
                return null;
            }

            switch ((LiturgieItemSoort)itemDto.Soort)
            {
                case (LiturgieItemSoort.LIED):
                    return new LiedItem();
                case (LiturgieItemSoort.SCHRIFTLEZING):
                    return new SchriftlezingItem();
                default:
                    return null;
            }
        }

        public class LiturgieDto
        {
            public long? Id { get; set; }
            [Required]
            public string Titel { get; set; }
            [Required]
            public DateTime Aanvangsdatum { get; set; }
            [Required]
            public DateTime Publicatiedatum { get; set; }
            public int? AantalItems { get; set; }
            public IEnumerable<LiturgieItemDto> Items { get; set; }

            public LiturgieDto() { }

            public LiturgieDto(Liturgie liturgie)
            {
                Id = liturgie.Id;
                Titel = liturgie.Titel;
                Aanvangsdatum = liturgie.Aanvangsdatum;
                Publicatiedatum = liturgie.Publicatiedatum;
                AantalItems = liturgie.AantalItems;
                Items = liturgie.Items?.Select(i => new LiturgieItemDto(i)) ?? new List<LiturgieItemDto>();
            }
        }

        public class LiturgieItemDto
        {
            public long? Id { get; set; }
            [Required]
            public int Index { get; set; }
            [Required]
            public int Soort { get; set; }

            public LiturgieItemDto() { }

            public LiturgieItemDto(LiturgieItem item)
            {
                Id = item.Id;
                Index = item.Index;
                Soort = (int)item.Soort;
            }
        }
    }
}
