using System.Collections.Generic;
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

        [HttpGet]
        public IActionResult Get()
        {
            var liturgieen = _liturgieRepository.GetLiturgieen();
            return Ok(liturgieen);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var liturgie = _liturgieRepository.GetLiturgie(id);

            if (liturgie == null)
            {
                return NotFound("Liturgie niet gevonden.");
            }

            return Ok(liturgie);
        }
    }
}
