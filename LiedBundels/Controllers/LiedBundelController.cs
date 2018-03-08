using LiturgieMakerAPI.Liedbundels.Model;
using LiturgieMakerAPI.Liedbundels.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LiturgieMakerAPI.Liedbundels.Controllers
{
    [Route("api/[controller]")]
    public class LiedbundelController : Controller
    {
        private readonly LiedbundelRepository _repository;

        public LiedbundelController(LiedbundelRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Alle liedbundels ophalen
        /// </summary>
        /// <remarks>
        /// Haalt de liederen zelf niet op
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Liedbundel[]), 200)]
        public IActionResult Get()
        {
            var liedbundels = _repository.GetLiedbundels();

            return Ok(liedbundels);
        }

        /// <summary>
        /// Een enkel liedbundel ophalen
        /// </summary>
        /// <param name="id">Uniek ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Liedbundel), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public IActionResult Get(int id)
        {
            var liedbundel = _repository.GetLiedbundel(id);

            if (liedbundel == null)
            {
                return NotFound("Liedbundel niet gevonden");
            }

            return Ok(liedbundel);
        }

        /// <summary>
        /// Zoeken naar liedbundel
        /// </summary>
        /// <remarks>
        /// Case-insensitive
        /// </remarks>
        /// <param name="zoekTerm">Naam van liedbundel</param>
        /// <returns></returns>
        [HttpGet("[action]/{zoekTerm}")]
        [ProducesResponseType(typeof(Liedbundel[]), 200)]
        [ProducesResponseType(204)]
        public IActionResult Search(string zoekTerm)
        {
            var liedbundels = _repository.SearchLiedbundel(zoekTerm);

            if (!liedbundels.Any())
            {
                return NoContent();
            }

            return Ok(liedbundels);
        }
    }
}
