using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestTienda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetById(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Cliente cliente)
        {
            var id = await _clienteService.CreateClienteAsync(cliente);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            var result = await _clienteService.UpdateClienteAsync(cliente);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clienteService.DeleteClienteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
