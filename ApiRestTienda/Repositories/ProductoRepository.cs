using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Infrastructure.Data;
using Dapper;

namespace ApiRestTienda.Repositories
{
    public class ProductoRepository : IRepository<Producto>
    {
        private readonly DatabaseConnection _db;

        public ProductoRepository(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            using (var connection = _db.CreateConnection())
            {
                return await connection.QueryAsync<Producto>("SELECT * FROM Productos");
            }
        }

        public async Task<Producto> GetByIdAsync(int id)
        {
            using (var connection = _db.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Producto>(
                    "SELECT * FROM Productos WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<int> AddAsync(Producto producto)
        {
            using (var connection = _db.CreateConnection())
            {
                string sql = @"
                    INSERT INTO Productos (Nombre, Precio, Stock) 
                    VALUES (@Nombre, @Precio, @Stock);
                    SELECT LAST_INSERT_ID();";

                return await connection.ExecuteScalarAsync<int>(sql, producto);
            }
        }

        public async Task<bool> UpdateAsync(Producto producto)
        {
            using (var connection = _db.CreateConnection())
            {
                string sql = @"
                    UPDATE Productos 
                    SET Nombre = @Nombre, 
                        Precio = @Precio, 
                        Stock = @Stock
                    WHERE Id = @Id";

                int rowsAffected = await connection.ExecuteAsync(sql, producto);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _db.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(
                    "DELETE FROM Productos WHERE Id = @Id", new { Id = id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> ActualizarStockAsync(int id, int cantidad)
        {
            using (var connection = _db.CreateConnection())
            {
                string sql = @"
                    UPDATE Productos 
                    SET Stock = Stock - @Cantidad
                    WHERE Id = @Id AND Stock >= @Cantidad";

                int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, Cantidad = cantidad });
                return rowsAffected > 0;
            }
        }
    }
}
