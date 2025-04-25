namespace ChatAI.Services
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using ChatAI.Models;
    using Microsoft.Extensions.Options;

    namespace HuggingFaceAIService.Services
    {
        public class HuggingFaceService : IAIService
        {
            private readonly HttpClient _httpClient;
            private readonly HuggingFaceSettings _settings;
            private readonly ILogger<HuggingFaceService> _logger;

            public HuggingFaceService(
                HttpClient httpClient,
                IOptions<HuggingFaceSettings> settings,
                ILogger<HuggingFaceService> logger)
            {
                _httpClient = httpClient;
                _settings = settings.Value;
                _logger = logger;
            }

            public async Task<AIResponse> GetResponseAsync(AIRequest request)
            {
                try
                {
                    var url = _settings.ChatEndpoint;
                    var token = _settings.ApiKey;

                    var messages = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "role", "user" },
                            { "content", request.Input }
                        }
                    };

                    var huggingFaceRequest = new HuggingFaceRequest
                    {
                        model = _settings.DefaultModel,
                        messages = messages,
                        temperature = 0.5,
                        max_tokens = 2048,
                        top_p = 0.7,
                        stream = false 
                    };

                    var json = JsonSerializer.Serialize(huggingFaceRequest);
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);

                    var result = await response.Content.ReadAsStringAsync();

                    var document = JsonDocument.Parse(result);

                    var resultJson = document
                        .RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    Console.WriteLine(resultJson);


                    return new AIResponse { Answer = resultJson };

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al comunicarse con Hugging Face API");
                    throw;
                }
            }
        }
    }
}
