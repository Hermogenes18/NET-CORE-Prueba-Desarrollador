using ApiRestTienda.Domain.Models;
using ApiRestTienda.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestTienda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost]
        public async Task<ActionResult<ChatbotResponse>> EnviarMensaje(ChatbotRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Mensaje))
            {
                return BadRequest(new { message = "El mensaje no puede estar vacío" });
            }

            var respuesta = await _chatbotService.ProcesarPreguntaAsync(request.Mensaje);

            return Ok(new ChatbotResponse { Respuesta = respuesta });
        }
    }
}
