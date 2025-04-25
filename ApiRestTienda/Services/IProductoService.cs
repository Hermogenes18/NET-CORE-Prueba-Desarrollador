using ApiRestTienda.Domain.Entities;

namespace ApiRestTienda.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetAllProductosAsync();
        Task<Producto> GetProductoByIdAsync(int id);
        Task<int> CreateProductoAsync(Producto producto);
        Task<bool> UpdateProductoAsync(Producto producto);
        Task<bool> DeleteProductoAsync(int id);
    }
}
