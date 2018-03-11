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
        private const string ERROR_NIET_VALIDE_LITURGIE = "De opgestuurde liturgie voldoet niet aan de specificaties.";

        private readonly LiturgieRepository _liturgieRepository;

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
        [ProducesResponseType(typeof(LiturgieDto[]), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            var liturgieen = _liturgieRepository.GetLiturgieen();
            return Ok(Mapper.Map<IEnumerable<LiturgieDto>>(liturgieen));
        }

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
                return NotFound("Deze liturgie bestaat niet.");
            }

            return Ok(Mapper.Map<LiturgieDto>(liturgie));
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

            var liturgie = _liturgieRepository.SaveLiturgie(Mapper.Map<Liturgie>(liturgieDto));

            return CreatedAtAction("Get", new { id = liturgieDto.Id }, Mapper.Map<LiturgieDto>(liturgie));
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
            if (liturgieDto == null || !TryValidateModel(liturgieDto))
            {
                return BadRequest(ERROR_NIET_VALIDE_LITURGIE);
            }

            _liturgieRepository.SaveLiturgie(Mapper.Map<Liturgie>(liturgieDto));

            return NoContent();
        }

        /// <summary>
        /// Een liturgie verwijderen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 403)]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] long id)
        {
            _liturgieRepository.DeleteLiturgie(_liturgieRepository.GetLiturgie(id));
            return NoContent();
        }
    }
}
