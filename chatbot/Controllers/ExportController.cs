using chatbot.Services;
using Microsoft.AspNetCore.Mvc;
using chatbot.Models;

namespace chatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExportController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpGet("export")]
        public IActionResult Export()
        {
            var mock = new List<ProductResult>
            {
                new() { Reference = "123", Title = "Titre A", Description = "Desc A", Tags = new() {"x", "y"} }
            };
            _excelService.Export(mock, "export.xlsx");
            return Ok("Export terminé.");
        }
    }
}