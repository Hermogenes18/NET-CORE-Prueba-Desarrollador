using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Infrastructure.Data;
using Dapper;

namespace ApiRestTienda.Repositories
{
    public class PedidoRepository : IRepository<Pedido>
    {
        private readonly DatabaseConnection _db;

        public PedidoRepository(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            using (var connection = _db.CreateConnection())
            {
                var pedidosDict = new Dictionary<int, Pedido>();

                var result = await connection.QueryAsync<Pedido, DetallePedido, Pedido>(
                    @"SELECT p.*, d.* 
                      FROM Pedidos p
                      LEFT JOIN DetallesPedido d ON p.Id = d.PedidoId",
                    (pedido, detalle) =>
                    {
                        if (!pedidosDict.TryGetValue(pedido.Id, out var pedidoExistente))
                        {
                            pedidoExistente = pedido;
                            pedidosDict.Add(pedido.Id, pedidoExistente);
                        }

                        if (detalle != null)
                        {
                            pedidoExistente.Detalles.Add(detalle);
                        }

                        return pedidoExistente;
                    },
                    splitOn: "Id");

                return pedidosDict.Values;
            }
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            using (var connection = _db.CreateConnection())
            {
                var pedidosDict = new Dictionary<int, Pedido>();

                var result = await connection.QueryAsync<Pedido, DetallePedido, Pedido>(
                    @"SELECT p.*, d.* 
                      FROM Pedidos p
                      LEFT JOIN DetallesPedido d ON p.Id = d.PedidoId
                      WHERE p.Id = @Id",
                    (pedido, detalle) =>
                    {
                        if (!pedidosDict.TryGetValue(pedido.Id, out var pedidoExistente))
                        {
                            pedidoExistente = pedido;
                            pedidosDict.Add(pedido.Id, pedidoExistente);
                        }

                        if (detalle != null)
                        {
                            pedidoExistente.Detalles.Add(detalle);
                        }

                        return pedidoExistente;
                    },
                    new { Id = id },
                    splitOn: "Id");

                return pedidosDict.Values.FirstOrDefault();
            }
        }

        public async Task<int> AddAsync(Pedido pedido)
        {
            using (var connection = _db.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                       
                        string sqlPedido = @"
                            INSERT INTO Pedidos (ClienteId, Fecha, Total) 
                            VALUES (@ClienteId, @Fecha, @Total);
                            SELECT LAST_INSERT_ID();";

                        int pedidoId = await connection.ExecuteScalarAsync<int>(sqlPedido, pedido, transaction);

                        
                        if (pedido.Detalles != null && pedido.Detalles.Any())
                        {
                            string sqlDetalle = @"
                                INSERT INTO DetallesPedido (PedidoId, ProductoId, Cantidad, PrecioUnitario, Subtotal) 
                                VALUES (@PedidoId, @ProductoId, @Cantidad, @PrecioUnitario, @Subtotal)";

                            foreach (var detalle in pedido.Detalles)
                            {
                                detalle.PedidoId = pedidoId;
                                await connection.ExecuteAsync(sqlDetalle, detalle, transaction);
                            }
                        }

                        transaction.Commit();
                        return pedidoId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<bool> UpdateAsync(Pedido pedido)
        {
            using (var connection = _db.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        
                        string sqlPedido = @"
                            UPDATE Pedidos 
                            SET ClienteId = @ClienteId, 
                                Fecha = @Fecha, 
                                Total = @Total
                            WHERE Id = @Id";

                        await connection.ExecuteAsync(sqlPedido, pedido, transaction);

                        
                        await connection.ExecuteAsync(
                            "DELETE FROM DetallesPedido WHERE PedidoId = @PedidoId",
                            new { PedidoId = pedido.Id },
                            transaction);

                        
                        if (pedido.Detalles != null && pedido.Detalles.Any())
                        {
                            string sqlDetalle = @"
                                INSERT INTO DetallesPedido (PedidoId, ProductoId, Cantidad, PrecioUnitario, Subtotal) 
                                VALUES (@PedidoId, @ProductoId, @Cantidad, @PrecioUnitario, @Subtotal)";

                            foreach (var detalle in pedido.Detalles)
                            {
                                detalle.PedidoId = pedido.Id;
                                await connection.ExecuteAsync(sqlDetalle, detalle, transaction);
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _db.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        
                        await connection.ExecuteAsync(
                            "DELETE FROM DetallesPedido WHERE PedidoId = @Id",
                            new { Id = id },
                            transaction);

                        
                        int rowsAffected = await connection.ExecuteAsync(
                            "DELETE FROM Pedidos WHERE Id = @Id",
                            new { Id = id },
                            transaction);

                        transaction.Commit();
                        return rowsAffected > 0;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
