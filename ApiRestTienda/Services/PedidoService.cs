using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Repositories;

namespace ApiRestTienda.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IRepository<Pedido> _pedidoRepository;
        private readonly ProductoRepository _productoRepository;

        public PedidoService(IRepository<Pedido> pedidoRepository, ProductoRepository productoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _productoRepository = productoRepository;
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidosAsync()
        {
            return await _pedidoRepository.GetAllAsync();
        }

        public async Task<Pedido> GetPedidoByIdAsync(int id)
        {
            return await _pedidoRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePedidoAsync(Pedido pedido)
        {
           
            decimal total = 0;

            foreach (var detalle in pedido.Detalles)
            {
                var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);

                if (producto == null)
                {
                    throw new Exception($"El producto con ID {detalle.ProductoId} no existe.");
                }

                if (producto.Stock < detalle.Cantidad)
                {
                    throw new Exception($"Stock insuficiente para el producto {producto.Nombre}. Stock actual: {producto.Stock}, solicitado: {detalle.Cantidad}");
                }

                
                detalle.PrecioUnitario = producto.Precio;
                detalle.Subtotal = producto.Precio * detalle.Cantidad;

                total += detalle.Subtotal;
            }

            
            pedido.Total = total;
            pedido.Fecha = DateTime.Now;

            
            var pedidoId = await _pedidoRepository.AddAsync(pedido);

            
            foreach (var detalle in pedido.Detalles)
            {
                await _productoRepository.ActualizarStockAsync(detalle.ProductoId, detalle.Cantidad);
            }

            return pedidoId;
        }

        public async Task<bool> UpdatePedidoAsync(Pedido pedido)
        {
            return await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task<bool> DeletePedidoAsync(int id)
        {
            return await _pedidoRepository.DeleteAsync(id);
        }
    }
}