using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public interface IUTurnajRepository
    {
        Task<IEnumerable<Turnaj>> GetAllAsync();
        Task<IEnumerable<Turnaj>> GetUpcomingTournamentsAsync();
        Task<IEnumerable<Turnaj>> GetUpcomingTournamentsByOwnerAsync(int? id);
        Task<Turnaj> GetByIdAsync(int id);
        Task<int> AddAsync(Turnaj model);
        Task<bool> UpdateAsync(Turnaj model);
        Task<bool> DeleteAsync(int id);
    }
}