using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestTienda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetAll()
        {
            var pedidos = await _pedidoService.GetAllPedidosAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetById(int id)
        {
            var pedido = await _pedidoService.GetPedidoByIdAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Pedido pedido)
        {
            try
            {
                var id = await _pedidoService.CreatePedidoAsync(pedido);
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            var result = await _pedidoService.UpdatePedidoAsync(pedido);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pedidoService.DeletePedidoAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}