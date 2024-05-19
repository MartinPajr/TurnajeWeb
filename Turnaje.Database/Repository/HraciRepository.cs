using Dapper;
using System.Data;
using Turnaje.Database.Entity;


namespace Turnaje.Database.Repository
{
    public class HraciRepository : IUHraciRepository
    {
        private readonly IDbConnection _connection;
        public HraciRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> AddAsync(Hraci model)
        {
            var sql = "INSERT INTO Hraci (Turnaj, Uzivatel) VALUES (@TurnajId, @UzivatelId)";
            return await _connection.ExecuteScalarAsync<int>(sql, model);
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Hraci>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Hraci> GetByIdAsync(int turnajId, int uzivatelId)
        {
            var query = "SELECT Turnaj, Uzivatel FROM Hraci WHERE Turnaj = @TurnajId AND Uzivatel = @UzivatelId";
            return await _connection.QueryFirstOrDefaultAsync<Hraci>(query, new { UzivatelId = uzivatelId, TurnajId = turnajId });
        }

        public Task<bool> UpdateAsync(Hraci model)
        {
            throw new NotImplementedException();
        }
    }
}
