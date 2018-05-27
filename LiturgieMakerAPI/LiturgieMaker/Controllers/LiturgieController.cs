using AutoMapper;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LiturgieMakerAPI.LiturgieMaker.Controllers
{
    [Route("api/[controller]")]
    public class LiturgieController : Controller
    {
        public const string ERROR_NIET_VALIDE_LITURGIE = "De opgestuurde liturgie voldoet niet aan de specificaties.";
        public const string ERROR_LITURGIE_BESTAAT_NIET = "Deze liturgie bestaat niet.";
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
        [ProducesResponseType(typeof(LiturgieDto[]), 200)]
        [HttpGet("{page}/{results}")]
        public IActionResult Get([FromRoute] int page, [FromRoute] int results) => Ok(_mapper.Map<IEnumerable<LiturgieDto>>(_liturgieRepository.GetLiturgieen(page, results)));

        /// <summary>
        /// Aantal liturgieen van deze gebruiker ophalen
        /// </summary>
        /// <returns>Aantal liturgieen</returns>
        [ProducesResponseType(typeof(int), 200)]
        [HttpGet("aantal")]
        public IActionResult GetAantal() => Ok(_liturgieRepository.GetAantalLitugieen());

        /// <summary>
        /// Een enkele liturgie ophalen
        /// </summary>
        /// <remarks>
        /// Haalt ook items en teksten op
        /// </remarks>
        /// <param name="id">Uniek ID</param>
        /// <returns>Een liturgie</returns>
        [ProducesResponseType(typeof(LiturgieDto), 200)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] long id)
        {
            var liturgie = _liturgieRepository.GetLiturgie(id);

            if (liturgie == null)
            {
                return NotFound(ERROR_LITURGIE_BESTAAT_NIET);
            }

            var dto = _mapper.Map<LiturgieDto>(liturgie);

            return Ok(dto);
        }

        /// <summary>
        /// Maak een nieuwe liturgie aan
        /// </summary>
        /// <param name="liturgieDto"></param>
        /// <returns>Aangemaakte liturgie met Get locatie in de header</returns>
        [ProducesResponseType(typeof(LiturgieDto), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost]
        public IActionResult Post([FromBody] LiturgieDto liturgieDto)
        {
            if (liturgieDto == null || liturgieDto.Id != null || !TryValidateModel(liturgieDto))
            {
                return BadRequest(ERROR_NIET_VALIDE_LITURGIE);
            }

            var liturgie = _liturgieRepository.SaveLiturgie(_mapper.Map<Liturgie>(liturgieDto));

            return CreatedAtAction("Get", new { id = liturgie.Id }, _mapper.Map<LiturgieDto>(liturgie));
        }

        /// <summary>
        /// Vervang een bestaand liturgie met een nieuwe versie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="liturgieDto"></param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] long id, [FromBody] LiturgieDto liturgieDto)
        {
            if (liturgieDto == null || id != liturgieDto.Id || !TryValidateModel(liturgieDto))
            {
                return BadRequest(ERROR_NIET_VALIDE_LITURGIE);
            }

            _liturgieRepository.SaveLiturgie(_mapper.Map<Liturgie>(liturgieDto));

            return NoContent();
        }

        /// <summary>
        /// Een liturgie verwijderen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] long id)
        {
            var liturgie = _liturgieRepository.GetLiturgie(id);

            if (liturgie == null)
            {
                return NotFound(ERROR_LITURGIE_BESTAAT_NIET);
            }

            _liturgieRepository.DeleteLiturgie(liturgie);
            return NoContent();
        }
    }
}
