using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Repositories;

namespace ApiRestTienda.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IRepository<Producto> _productoRepository;

        public ProductoService(IRepository<Producto> productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            return await _productoRepository.GetAllAsync();
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            return await _productoRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateProductoAsync(Producto producto)
        {
            return await _productoRepository.AddAsync(producto);
        }

        public async Task<bool> UpdateProductoAsync(Producto producto)
        {
            return await _productoRepository.UpdateAsync(producto);
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            return await _productoRepository.DeleteAsync(id);
        }
    }
}
