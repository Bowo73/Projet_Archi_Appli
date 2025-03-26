using chatbot.Models;
using chatbot.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("import")]
        public IActionResult Import()
        {
            var products = _excelService.Import("products.xlsx");
            foreach (var p in products)
                _queueService.Enqueue(p);

            return Ok($"{products.Count} produits importés dans la file");
        }
    }
}