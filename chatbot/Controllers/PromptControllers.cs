// Controllers/PromptController.cs
using chatbot.Services;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromptController : ControllerBase
    {
        private readonly ChatOrchestratorService _chatOrchestrator;

        public PromptController(ChatOrchestratorService chatOrchestrator)
        {
            _chatOrchestrator = chatOrchestrator;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendPrompt([FromForm] IFormFile? file, [FromForm] string message)
        {
            try
            {
                var result = await _chatOrchestrator.ProcessUserPromptAsync(message, file);
                return Ok(new { content = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur côté serveur : {ex.Message}");
            }
        }
    }
}