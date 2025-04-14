using chatbot.Services;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerateController : ControllerBase
    {
        private readonly IGenerationService _generationService;

        public GenerateController(IGenerationService generationService)
        {
            _generationService = generationService;
        }

        [HttpPost("run")]
        public async Task<IActionResult> Generate()
        {
            var result = await _generationService.GenerateAsync();
            return Ok(result);
        }
    }
}