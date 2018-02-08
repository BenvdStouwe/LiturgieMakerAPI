using System;
using System.Collections.Generic;
using System.Linq;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LiturgieMakerAPI.LiturgieMaker.Controllers
{
    [Route("api/[controller]")]
    public class LiturgieController : Controller
    {
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
        [ProducesResponseType(typeof(string), 404)]
        public IActionResult Get(long id)
        {
            var liturgie = _liturgieRepository.GetLiturgie(id);

            if (liturgie == null)
            {
                return NotFound("Liturgie niet gevonden.");
            }

            return Ok(new LiturgieDto(liturgie));
        }

        private class LiturgieDto
        {
            public long Id { get; set; }
            public string Titel { get; set; }
            public DateTime Aanvangsdatum { get; set; }
            public DateTime Publicatiedatum { get; set; }
            public int AantalItems { get; set; }
            public IEnumerable<LiturgieItemDto> Items { get; set; }

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

        private class LiturgieItemDto
        {
            public long Id { get; set; }
            public int Index { get; set; }
            public string Soort { get; set; }

            public LiturgieItemDto(LiturgieItem item)
            {
                Id = item.Id;
                Index = item.Index;
                Soort = item.Soort.ToString();
            }
        }
    }
}
