using ApiRestTienda.Domain.Entities;

namespace ApiRestTienda.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientesAsync();
        Task<Cliente> GetClienteByIdAsync(int id);
        Task<int> CreateClienteAsync(Cliente cliente);
        Task<bool> UpdateClienteAsync(Cliente cliente);
        Task<bool> DeleteClienteAsync(int id);
    }
}
