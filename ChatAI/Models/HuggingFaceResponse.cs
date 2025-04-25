namespace ChatAI.Models
{
    public class HuggingFaceResponse
    {
        public List<Dictionary<string, double>>? Scores { get; set; }
        public List<string>? Generated_Text { get; set; }
    }
}
