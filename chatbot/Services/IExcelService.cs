using chatbot.Models;

namespace chatbot.Services
{
    public interface IExcelService
    {
        List<ProductRequest> Import(string filePath);
        void Export(List<ProductResult> results, string filePath);
    }

    public class ExcelService : IExcelService
    {
        public List<ProductRequest> Import(string filePath)
        {
            // Simulé : importer des produits depuis Excel
            return new List<ProductRequest>
            {
                new ProductRequest { Reference = "123", Characteristics = new Dictionary<string, string>{{"color", "red"}} },
                new ProductRequest { Reference = "456", Characteristics = new Dictionary<string, string>{{"color", "blue"}} }
            };
        }

        public void Export(List<ProductResult> results, string filePath)
        {
            // Simulé : export des résultats vers Excel
        }
    }
}