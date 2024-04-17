using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public interface IUzivatelRepository
    {
        Task<IEnumerable<Uzivatel>> GetAllAsync();
        Task<Uzivatel> GetByIdAsync(int id);
        Task<int> AddAsync(Uzivatel model);
        Task<bool> UpdateAsync(Uzivatel model);
        Task<bool> DeleteAsync(int id);
        Task<List<Uzivatel>> GetByTournamentId(int id);
        Task <Uzivatel> GetByName(string nickname);
        Task <List<Hra>> GetHryUzivatel(int id);
    }
}
