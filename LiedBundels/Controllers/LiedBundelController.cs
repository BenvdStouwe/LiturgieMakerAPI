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

        [HttpGet]
        public IActionResult Get()
        {
            var liedbundels = _repository.GetLiedbundels();

            return Ok(liedbundels);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var liedbundel = _repository.GetLiedbundel(id);

            if (liedbundel == null)
            {
                return NotFound("Liedbundel niet gevonden");
            }

            return Ok(liedbundel);
        }
    }
}
