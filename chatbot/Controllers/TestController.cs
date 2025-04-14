using Microsoft.AspNetCore.Mvc;
using chatbot.Services;

namespace chatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IAiService _aiService;

        public TestController(IAiService aiService)
        {
            _aiService = aiService;
        }

        /// <summary>
        /// Génère une réponse depuis Azure OpenAI (ChatGPT)
        /// </summary>
        /// <param name="prompt">Texte à envoyer au modèle IA</param>
        /// <returns>Contenu généré</returns>
        [HttpGet("generate")]
        public async Task<IActionResult> Generate([FromQuery] string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return BadRequest("Le prompt ne peut pas être vide.");

            var result = await _aiService.GenerateTextAsync(prompt);
            return Ok(new { result });
        }
    }
}
