using ChatAI.Models;

namespace ChatAI.Services
{
    public interface IAIService
    {
        Task<AIResponse> GetResponseAsync(AIRequest request);
    }
}
