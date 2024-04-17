using Dapper;
using System.Data;
using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public class ZapasRepository : IUZapasRepository
    {
        private readonly IDbConnection _connection;

        public ZapasRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Zapas>> GetAllAsync()
        {
            return await _connection.QueryAsync<Zapas>("SELECT * FROM Zapas");
        }

        public async Task<Zapas> GetByIdAsync(int id)
        {
            var query = "SELECT Id, Turnaj, H1 as Player1, H2 as Player2, Predzapas1, Predzapas2, H1Score as P1Score, H2Score as P2Score, Winner, RoundNumber FROM Zapas WHERE Id = @Id";
            return (Zapas)await _connection.QueryFirstOrDefaultAsync<Zapas>(query, new { Id = id });
        }
        public async Task<List<Zapas>> GetByTournamentId(int id)
        {
            var query = "SELECT Id, Turnaj, H1 as Player1, H2 as Player2, Predzapas1, Predzapas2, H1Score as P1Score, H2Score as P2Score, Winner, RoundNumber FROM Zapas WHERE Turnaj = @Id";
            return (List<Zapas>)await _connection.QueryAsync<Zapas>(query, new { Id = id });
        }
        public async Task<List<Zapas>> GetByTournamentAndRoundNumberId(int id, int roundNumber)
        {
            var query = "SELECT Id, Turnaj, H1 as Player1, H2 as Player2, Predzapas1, Predzapas2, H1Score as P1Score, H2Score as P2Score, Winner, RoundNumber FROM Zapas WHERE Turnaj = @Id AND RoundNumber = @RoundNumber";
            return (List<Zapas>)await _connection.QueryAsync<Zapas>(query, new { Id = id, RoundNumber = roundNumber });
        }


        public async Task<int> AddAsync(Zapas model)
        {
            var sql = "INSERT INTO Zapas (Turnaj, H1, H2, H1Score, H2Score, Winner, RoundNumber, Predzapas1, Predzapas2) VALUES (@Turnaj, @Player1, @Player2, @H1Score, @H2Score, @Winner, @RoundNumber, @Predzapas1, @Predzapas2); SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _connection.ExecuteScalarAsync<int>(sql, model);
        }

        public async Task<bool> UpdateAsync(Zapas model)
        {
            var sql = "UPDATE Zapas SET Turnaj = @Turnaj, H1 = @Player1, H2 = @Player2, H1Score = @H1Score, H2Score = @H2Score, Winner = @Winner WHERE Id = @Id";
            var result = await _connection.ExecuteAsync(sql, model);
            return result > 0;
        }
        public async Task<Zapas> GetByRoundBeforeMatch(int id)
        {
            var query = "SELECT Id, Turnaj, H1 as Player1, H2 as Player2, Predzapas1, Predzapas2, H1Score, H2Score, Winner, RoundNumber, Winner, PredZapas1, PredZapas2 FROM Zapas WHERE Predzapas1 = @Id OR Predzapas2 = @Id";
            var result = await _connection.QueryAsync<Zapas>(query, new { Id = id });
            return result.FirstOrDefault();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _connection.ExecuteAsync("DELETE FROM Zapas WHERE Turnaj = @Id", new { Id = id });
            return result > 0;
        }
    }
}
