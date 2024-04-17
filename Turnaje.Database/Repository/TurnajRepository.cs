using Dapper;
using System.Data;
using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public class TurnajRepository : IUTurnajRepository
    {
        private readonly IDbConnection _connection;

        public TurnajRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Turnaj>> GetAllAsync()
        {
            return await _connection.QueryAsync<Turnaj>("SELECT * FROM Turnaj");
        }

        public async Task<Turnaj> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Turnaj WHERE Id = @Id";
            var result = await _connection.QueryFirstOrDefaultAsync<Turnaj>(sql, new { Id = id });
            return result;
        }

        public async Task<int> AddAsync(Turnaj model)
        {
            var sql = "INSERT INTO Turnaj (Name, MaxPlayers, Style, Game, Description, Seed, Start, CheckStart, PocetKol, Majitel, Pravidla) VALUES (@Name, @MaxPlayers, @Style, @Game, @Description, @Seed, @Start,@CheckStart, @PocetKol, @Majitel, @Pravidla); SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _connection.ExecuteScalarAsync<int>(sql, model);
        }

        public async Task<bool> UpdateAsync(Turnaj model)
        {
            var sql = "UPDATE Turnaj SET Name = @Name, Description = @Description, Start = @Start, CheckStart = @CheckStart WHERE Id = @Id";
            var result = await _connection.ExecuteAsync(sql, model);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _connection.ExecuteAsync("DELETE FROM Turnaj WHERE Id = @Id", new { Id = id });
            return result > 0;
        }
    }
}
