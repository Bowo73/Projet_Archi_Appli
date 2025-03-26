using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Net.Http;
using System.Text.Json;
using ChatBot.Data;
using ChatBot.Models;

namespace ChatBot.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ChatBotController : ControllerBase
    {
        private readonly ChatBotDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatBotController(ChatBotDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportProducts([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Fichier invalide");

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    List<Models.Product> products = new();
                    
                    int rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var product = new ChatBot.Models.Product
                        {
                            Name = worksheet.Cells[row, 1].Value?.ToString() ?? string.Empty,
                            Category = worksheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                            Description = worksheet.Cells[row, 3].Value?.ToString() ?? ""
                        };
                        products.Add(product);
                    }
                    
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }
            return Ok("Produits importés avec succès");
        }

        [HttpPost("generate-descriptions")]
        public async Task<IActionResult> GenerateDescriptions()
        {
            var products = await _context.Products.Where(p => p.GeneratedDescription == null).ToListAsync();
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer VOTRE_CLE_AZURE");
            
            foreach (var product in products)
            {
                var requestBody = new { prompt = $"Génère une description marketing pour {product.Name} dans la catégorie {product.Category}" };
                var response = await client.PostAsJsonAsync("https://az-dev-fc-epsi-cog-002-xfq.openai.azure.com/deployments/gpt35/chat/completions", requestBody);
                var responseBody = await response.Content.ReadAsStringAsync();
                
                var responseData = JsonSerializer.Deserialize<OpenAIResponse>(responseBody);
                product.GeneratedDescription = responseData?.Choices.FirstOrDefault()?.Text;
            }
            
            await _context.SaveChangesAsync();
            return Ok("Descriptions générées");
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string? GeneratedDescription { get; set; }
    }

    public class OpenAIResponse
    {
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        public string Text { get; set; }
    }
}