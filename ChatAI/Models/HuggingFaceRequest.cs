namespace ChatAI.Models
{
    public class HuggingFaceRequest
    {
        public string model { get; set; } = string.Empty;
        public List<Dictionary<string, string>> messages { get; set; } = new();
        public double temperature { get; set; } = 0.5;
        public int max_tokens { get; set; } = 2048;
        public double top_p { get; set; } = 0.7;
        public bool stream { get; set; } = false;
    }
}
