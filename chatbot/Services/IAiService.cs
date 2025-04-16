using System.Net.Http.Json;
using System.Text.Json;
using chatbot.Configuration;
using Microsoft.Extensions.Options;

namespace chatbot.Services
{
    public interface IAiService
    {
        Task<string> GenerateTextAsync(string prompt);
    }

    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly AzureOpenAISettings _settings;

        public AiService(HttpClient httpClient, IOptions<AzureOpenAISettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;

            _httpClient.BaseAddress = new Uri(_settings.Endpoint); // ex : https://votre-instance.openai.azure.com/
            _httpClient.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);
        }
    public async Task<string> GenerateTextAsync(string prompt)
        {
            return await RetryAsync(async () =>
            {
                var requestUri = $"openai/deployments/{_settings.DeploymentId}/chat/completions?api-version={_settings.ApiVersion}";

                var requestBody = new
                {
                    messages = new[]
                    {
                        new { role = "system", content = "tu es une IA d'aide" },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.7,
                    max_tokens = 500
                };

                var response = await _httpClient.PostAsJsonAsync(requestUri, requestBody);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                return json.GetProperty("choices")[0]
                           .GetProperty("message")
                           .GetProperty("content")
                           .GetString() ?? string.Empty;
            }, maxRetries: 3, delay: TimeSpan.FromSeconds(2));
        }

        private async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxRetries, TimeSpan delay)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    attempt++;
                    if (attempt >= maxRetries)
                        throw;

                    Console.WriteLine($"Tentative {attempt}/{maxRetries} échouée : {ex.Message}");
                    await Task.Delay(delay);
                }
            }
        }
    }
}