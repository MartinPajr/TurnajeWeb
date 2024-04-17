using Dapper;
using System.Data;
using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public class HraRepository : IUHraRepository
    {
        private readonly IDbConnection _connection;
        public HraRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> AddAsync(Hra model)
        {
            var sql = "INSERT INTO Hry (Name) VALUES (@Name)";
            return await _connection.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<List<Hra>> GetAllAsync()
        {
            return (List<Hra>)await _connection.QueryAsync<Hra>("SELECT * FROM Hry");
        }
        public async Task<Hra> GetByName(string name)
        {
            var query = "SELECT * FROM Hry WHERE Name = @Name";
            return await _connection.QueryFirstOrDefaultAsync<Hra>(query, new { Name = name });
        }

        public async Task<Hra> GetByIdAsync(int id)
        {
            var query = "SELECT Id, Name FROM Hry WHERE Id = @Id";
            return (Hra)await _connection.QueryFirstOrDefaultAsync<Hra>(query, new { Id = id });
        }

        public async Task<List<Hra>> GetHryByUzivatelId(int id)
        {
            var query = "SELECT Hry.Id AS Id, Hry.Name AS Name, HryUzivatel.GameId AS GameId FROM Hry INNER JOIN HryUzivatel ON Hry.Id = HryUzivatel.HraId WHERE HryUzivatel.UzivatelId = @Id";
            return (List<Hra>)await _connection.QueryAsync<Hra>(query, new { Id = id });
        }

        public Task<bool> UpdateAsync(Hra model)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
