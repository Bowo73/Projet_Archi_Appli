// Services/ChatOrchestratorService.cs

using chatbot.Models;

namespace chatbot.Services
{
    public class ChatOrchestratorService
    {
        private readonly IAiService _aiService;
        private readonly IExcelService _excelService;
        private readonly ILogger<ChatOrchestratorService> _logger;

        public ChatOrchestratorService(
            IAiService aiService,
            IExcelService excelService,
            ILogger<ChatOrchestratorService> logger)
        {
            _aiService = aiService;
            _excelService = excelService;
            _logger = logger;
        }

        public async Task<string> ProcessUserPromptAsync(string message, IFormFile? file)
        {
            string finalPrompt = message;

            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var produits = _excelService.Import(stream);

                var produitTexte = string.Join("\n", produits.Select(p =>
                    $"{p.Reference} | {string.Join(" | ", p.Characteristics?.Select(c => $"{c.Key}: {c.Value}") ?? Enumerable.Empty<string>())}"));

                finalPrompt += $"\n\nVoici les produits à considérer :\n{produitTexte}";
            }

            return await RetryAsync(() => _aiService.GenerateTextAsync(finalPrompt), 3, TimeSpan.FromSeconds(2));
        }
        private async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxRetries, TimeSpan delay)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    attempts++;
                    _logger.LogWarning(ex, "Erreur tentative {Attempt}/{Max}.", attempts, maxRetries);
                    if (attempts >= maxRetries)
                        throw;
                    await Task.Delay(delay);
                }
            }
        }
    }
}