using Dapper;
using System.Data;
using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public class UzivatelRepository : IUzivatelRepository
    {
        private readonly IDbConnection _connection;

        public UzivatelRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Uzivatel>> GetAllAsync()
        {
            return await _connection.QueryAsync<Uzivatel>("SELECT * FROM Uzivatel");
        }

        public async Task<Uzivatel> GetByIdAsync(int id)
        {
            var sql = "SELECT *, C_Address as Crypto FROM Uzivatel WHERE Id = @Id";
            var result = await _connection.QueryFirstOrDefaultAsync<Uzivatel>(sql, new { Id = id });
            return result;
        }
        public async Task<Uzivatel> GetByName(string name)
        {
            var sql = "SELECT * FROM Uzivatel WHERE Nickname = @Name";
            var result = await _connection.QueryFirstOrDefaultAsync<Uzivatel>(sql, new { Name = name });
            return result;
        }

        public async Task<int> AddAsync(Uzivatel model)
        {
            var sql = "INSERT INTO Uzivatel (Nickname, Password, Email, C_Address) VALUES (@Nickname, @Password, @Email, @Crypto); SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _connection.ExecuteScalarAsync<int>(sql, model);
        }

        public async Task<bool> UpdateAsync(Uzivatel model)
        {
            var sql = "UPDATE Uzivatel SET Nickname = @Nickname, Password = @Password, Email = @Email, C_Address = @Crypto WHERE Id = @Id";
            var result = await _connection.ExecuteAsync(sql, model);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _connection.ExecuteAsync("DELETE FROM Uzivatel WHERE Id = @Id", new { Id = id });
            return result > 0;
        }
        public async Task<List<Uzivatel>> GetByTournamentId(int id)
        {
            var query = "SELECT Id,Nickname,Password,Email, C_Address FROM Uzivatel INNER JOIN Hraci ON Uzivatel.Id = Hraci.Uzivatel WHERE Hraci.Turnaj = @Id";
            return (List<Uzivatel>)await _connection.QueryAsync<Uzivatel>(query, new { Id = id });
        }
        public async Task<List<Hra>> GetHryUzivatel(int id)
        {
            var query = "SELECT * FROM HryUzivatel WHERE UzivatelId = @Id";
            return (List<Hra>)await _connection.QueryAsync<Hra>(query, new { Id = id });
        }

    }
}
