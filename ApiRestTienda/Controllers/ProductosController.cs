using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestTienda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetAll()
        {
            var productos = await _productoService.GetAllProductosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetById(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Producto producto)
        {
            var id = await _productoService.CreateProductoAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            var result = await _productoService.UpdateProductoAsync(producto);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productoService.DeleteProductoAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}