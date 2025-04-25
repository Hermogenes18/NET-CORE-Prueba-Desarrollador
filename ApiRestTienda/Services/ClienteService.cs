using ApiRestTienda.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiRestTienda.Repositories;

namespace ApiRestTienda.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IRepository<Cliente> _clienteRepository;

        public ClienteService(IRepository<Cliente> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<Cliente> GetClienteByIdAsync(int id)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateClienteAsync(Cliente cliente)
        {
            return await _clienteRepository.AddAsync(cliente);
        }

        public async Task<bool> UpdateClienteAsync(Cliente cliente)
        {
            return await _clienteRepository.UpdateAsync(cliente);
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            return await _clienteRepository.DeleteAsync(id);
        }
    }

}
