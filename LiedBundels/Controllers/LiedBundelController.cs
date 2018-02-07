using LiturgieMakerAPI.LiedBundels.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LiturgieMakerAPI.LiedBundels.Controllers
{
    [Route("api/[controller]")]
    public class LiedBundelController : Controller
    {
        private readonly LiedBundelRepository _repository;

        public LiedBundelController(LiedBundelRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var liedBundels = _repository.GetLiedbundels();

            return Ok(liedBundels);
        }
    }
}
