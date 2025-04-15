using chatbot.Models;
using chatbot.Services;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromptController : ControllerBase
    {
        private readonly IAiService _aiService;
        private readonly IExcelService _excelService;

        public PromptController(IAiService aiService, IExcelService excelService)
        {
            _aiService = aiService;
            _excelService = excelService;
        }

        /// <summary>
        /// Envoie un prompt au bot avec un fichier optionnel
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendPrompt([FromForm] string message, IFormFile? file)
        {
            if (string.IsNullOrWhiteSpace(message) && file == null)
                return BadRequest("Message ou fichier requis.");

            string finalPrompt = message;

            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var produits = _excelService.Import(stream);

                var produitTexte = string.Join("\n", produits.Select(p =>
                $"{p.Reference} | {string.Join(" | ", p.Characteristics?.Select(c => $"{c.Key}: {c.Value}") ?? Enumerable.Empty<string>())}"));


                finalPrompt += $"\n\nVoici les produits à considérer :\n{produitTexte}";
            }

            var result = await _aiService.GenerateTextAsync(finalPrompt);

            return Ok(new { content = result });
        }
    }
}
