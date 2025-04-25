using ApiRestTienda.Domain.Entities;

namespace ApiRestTienda.Services
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> GetAllPedidosAsync();
        Task<Pedido> GetPedidoByIdAsync(int id);
        Task<int> CreatePedidoAsync(Pedido pedido);
        Task<bool> UpdatePedidoAsync(Pedido pedido);
        Task<bool> DeletePedidoAsync(int id);
    }
}
