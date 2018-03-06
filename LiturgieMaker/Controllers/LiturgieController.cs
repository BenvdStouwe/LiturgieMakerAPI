using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Model.LiturgieItems;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LiturgieMakerAPI.LiturgieMaker.Controllers
{
    [Route("api/[controller]")]
    public class LiturgieController : Controller
    {
        private const string ERROR_GEVULD_ID_BIJ_POST = "Je probeerde een bestaande liturgie als nieuw op te sturen.";
        private const string ERROR_GEEN_VALIDE_LITURGIEITEMSOORT = "Eén of meer van de opgestuurde liturgie items had een onbekend type.";
        private const string ERROR_GEEN_TITEL = "Er is geen titel ingevuld.";
        private const string ERROR_GEEN_AANVANGSDATUM = "Er is geen aavangsdatum ingevuld.";
        private const string ERROR_GEEN_PUBLICATIEDATUM = "Er is geen publicatiedatum ingevuld.";

        private readonly LiturgieRepository _liturgieRepository;
        private readonly IMapper _mapper;

        public LiturgieController(LiturgieRepository liturgieRepository, IMapper mapper)
        {
            _liturgieRepository = liturgieRepository;
            _mapper = mapper;
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
            return Ok(liturgieen.Select(l => _mapper.Map<LiturgieDto>(l)));
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

            return Ok(_mapper.Map<LiturgieDto>(liturgie));
        }

        /// <summary>
        /// Maak een nieuwe liturgie aan
        /// </summary>
        /// <param name="liturgieDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(LiturgieDto), 201)]
        [ProducesResponseType(typeof(string[]), 400)]
        public IActionResult Post([FromForm]LiturgieDto liturgieDto)
        {
            if (!ValideerLiturgie(liturgieDto, out var errors))
            {
                return BadRequest(errors);
            }

            var liturgie = _mapper.Map<Liturgie>(liturgieDto);
            _liturgieRepository.SaveLiturgie(liturgie);
            return CreatedAtAction("Get", new { id = liturgie.Id }, _mapper.Map<LiturgieDto>(liturgie));
        }

        private bool ValideerLiturgie(LiturgieDto dto, out IList<string> errors)
        {
            errors = new List<string>();

            if (dto.Id != null)
            {
                errors.Add(ERROR_GEVULD_ID_BIJ_POST);
            }

            if (dto.Titel == null)
            {
                errors.Add(ERROR_GEEN_TITEL);
            }

            if (dto.Aanvangsdatum == null)
            {
                errors.Add(ERROR_GEEN_AANVANGSDATUM);
            }

            if (dto.Publicatiedatum == null)
            {
                errors.Add(ERROR_GEEN_PUBLICATIEDATUM);
            }

            if (dto.Items != null && dto.Items.Any(i => !Enum.IsDefined(typeof(LiturgieItemSoort), i.Soort)))
            {
                errors.Add(ERROR_GEEN_VALIDE_LITURGIEITEMSOORT);
            }

            return !errors.Any();
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
        public IEnumerable<LiturgieItemDto> Items { get; set; }
    }

    public class LiturgieItemDto
    {
        public long? Id { get; set; }
        [Required]
        public int Index { get; set; }
        [Required]
        [EnumDataType(typeof(LiturgieItemSoort))]
        [JsonConverter(typeof(StringEnumConverter))]
        public LiturgieItemSoort Soort { get; set; }
    }
}
