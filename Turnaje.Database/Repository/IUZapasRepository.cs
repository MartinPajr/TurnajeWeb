using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public interface IUZapasRepository
    {
        Task<IEnumerable<Zapas>> GetAllAsync();
        Task<Zapas> GetByIdAsync(int id);

        Task<int> AddAsync(Zapas model);
        Task<bool> UpdateAsync(Zapas model);
        Task<bool> DeleteAsync(int id);
        Task<List<Zapas>> GetByTournamentId(int id);

        Task<List<Zapas>> GetByTournamentAndRoundNumberId(int id, int roundNumber);
        Task<Zapas> GetByRoundBeforeMatch(int id);
    }
}