using chatbot.Models;
using ClosedXML.Excel;

namespace chatbot.Services
{
    public interface IExcelService
    {
        List<ProductRequest> Import(string filePath);
        void Export(List<ProductResult> results, string filePath);
        IEnumerable<ProductRequest?> Import(Stream stream);
    }

    public class ExcelService : IExcelService
    {
        public void Export(List<ProductResult> results, string filePath)
        {
            throw new NotImplementedException();
        }

        public List<ProductRequest> Import(Stream stream)
        {
            var result = new List<ProductRequest>();

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header row

            foreach (var row in rows)
            {
                var reference = row.Cell(1).GetString(); // Colonne A
                var color = row.Cell(2).GetString();     // Colonne B
                var size = row.Cell(3).GetString();      // Colonne C

                var characteristics = new Dictionary<string, string>
                {
                    { "color", color },
                    { "size", size }
                };

                result.Add(new ProductRequest
                {
                    Reference = reference,
                    Characteristics = characteristics
                });
            }

            return result;
        }

        public List<ProductRequest> Import(string filePath)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ProductRequest?> IExcelService.Import(Stream stream)
        {
            return Import(stream);
        }
    }
}
