using chatbot.Models;

namespace chatbot.Services
{
    public interface IGenerationService
    {
        Task<List<ProductResult>> GenerateAsync();
    }

    public class GenerationService : IGenerationService
    {
        private readonly IAiService _aiService;
        private readonly QueueService _queueService;

        public GenerationService(IAiService aiService, QueueService queueService)
        {
            _aiService = aiService;
            _queueService = queueService;
        }

        public async Task<List<ProductResult>> GenerateAsync()
        {
            var results = new List<ProductResult>();

            while (_queueService.HasItems)
            {
                var item = _queueService.Dequeue();
                if (item == null) continue;

                var prompt = string.Join(", ", item.Characteristics.Select(kv => $"{kv.Key}:{kv.Value}"));
                var generated = await _aiService.GenerateTextAsync(prompt);

                results.Add(new ProductResult
                {
                    Reference = item.Reference,
                    Title = $"Titre pour {item.Reference}",
                    Description = generated,
                    Tags = new List<string> {"tag1", "tag2"}
                });
            }

            return results;
        }
    }
}