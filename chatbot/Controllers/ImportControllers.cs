// Controllers/ImportController.cs
using Microsoft.AspNetCore.Mvc;
using chatbot.Services;

namespace chatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IExcelService _excelService;
        private readonly QueueService _queueService;

        public ImportController(IExcelService excelService, QueueService queueService)
        {
            _excelService = excelService;
            _queueService = queueService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Aucun fichier fourni.");

            using var stream = file.OpenReadStream();
            var products = _excelService.Import(stream); // Lire depuis le flux
            foreach (var p in products)
                _queueService.Enqueue(p);
            
            //var message = $"{products.Count} produits importés dans la file.";
            var message = "Les produits on été importés dans la file";
            return Ok(message);

           // return Ok($"{products.Count} produits importés dans la file.");
        }
    }
}
