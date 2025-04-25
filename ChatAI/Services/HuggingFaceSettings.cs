namespace ChatAI.Services
{
    public class HuggingFaceSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ChatEndpoint { get; set; } = string.Empty;
        public string DefaultModel { get; set; } = string.Empty;
    }
}
