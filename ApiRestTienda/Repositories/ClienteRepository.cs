using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Infrastructure.Data;

namespace ApiRestTienda.Repositories
{
    public class ClienteRepository : IRepository<Cliente>
    {
        private readonly DatabaseConnection _db;

        public ClienteRepository(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            using (var connection = _db.CreateConnection())
            {
                return await connection.QueryAsync<Cliente>("SELECT * FROM Clientes");
            }
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            using (var connection = _db.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Cliente>(
                    "SELECT * FROM Clientes WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<int> AddAsync(Cliente cliente)
        {
            using (var connection = _db.CreateConnection())
            {
                string sql = @"
                    INSERT INTO Clientes (Nombre, Correo, FechaNacimiento) 
                    VALUES (@Nombre, @Correo, @FechaNacimiento);
                    SELECT LAST_INSERT_ID();";

                return await connection.ExecuteScalarAsync<int>(sql, cliente);
            }
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            using (var connection = _db.CreateConnection())
            {
                string sql = @"
                    UPDATE Clientes 
                    SET Nombre = @Nombre, 
                        Correo = @Correo, 
                        FechaNacimiento = @FechaNacimiento
                    WHERE Id = @Id";

                int rowsAffected = await connection.ExecuteAsync(sql, cliente);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _db.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(
                    "DELETE FROM Clientes WHERE Id = @Id", new { Id = id });
                return rowsAffected > 0;
            }
        }
    }

}