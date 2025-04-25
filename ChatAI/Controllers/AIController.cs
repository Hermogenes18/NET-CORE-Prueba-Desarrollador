using ChatAI.Models;
using ChatAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;
        private readonly ILogger<AIController> _logger;

        public AIController(IAIService aiService, ILogger<AIController> logger)
        {
            _aiService = aiService;
            _logger = logger;
        }

        [HttpPost("response")]
        public async Task<ActionResult<AIResponse>> GetResponse(AIRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Input))
                {
                    return BadRequest("El campo 'input' no puede estar vacío.");
                }

                var response = await _aiService.GetResponseAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la solicitud de IA");
                return StatusCode(500, "Se produjo un error al procesar la solicitud.");
            }
        }
    }
}
