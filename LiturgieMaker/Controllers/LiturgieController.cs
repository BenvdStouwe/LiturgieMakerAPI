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
        private const string ERROR_NIET_VALIDE_LITURGIE = "De opgestuurde liturgie voeldoet niet aan de specificaties.";

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

            return Ok(Mapper.Map<LiturgieDto>(liturgie));
        }

        /// <summary>
        /// Maak een nieuwe liturgie aan
        /// </summary>
        /// <param name="liturgieDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(LiturgieDto), 201)]
        [ProducesResponseType(typeof(string[]), 400)]
        public IActionResult Post([FromBody] LiturgieDto liturgieDto)
        {
            if (liturgieDto == null || !TryValidateModel(liturgieDto))
            {
                return BadRequest(ERROR_NIET_VALIDE_LITURGIE);
            }

            var liturgie = _liturgieRepository.SaveLiturgie(Mapper.Map<Liturgie>(liturgieDto));

            return CreatedAtAction("Get", new { id = liturgieDto.Id }, Mapper.Map<LiturgieDto>(liturgie));
        }
    }
}
