using System.Linq;
using LiturgieMakerAPI.Liedbundels.Model;
using LiturgieMakerAPI.Liedbundels.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("search/{zoekTerm}")]
        public IActionResult Get(string zoekTerm)
        {
            var liedbundels = _repository.SearchLiedbundel(zoekTerm);

            if (!liedbundels.Any())
            {
                return NotFound("Geen liedbundel gevonden");
            }

            return Ok(liedbundels);
        }
    }
}
