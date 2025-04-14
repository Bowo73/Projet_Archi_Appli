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
            var requestUri = $"openai/deployments/{_settings.DeploymentId}/chat/completions?api-version={_settings.ApiVersion}";

            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "system", content = "Tu es un assistant marketing e-commerce." },
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
        }
    }
}
