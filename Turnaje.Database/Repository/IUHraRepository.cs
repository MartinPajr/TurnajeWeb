using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public interface IUHraRepository
    {
        Task<List<Hra>> GetAllAsync();
        Task<List<Hra>> GetHryByUzivatelId(int id);
        Task<Hra> GetByIdAsync(int id);
        Task<int> AddAsync(Hra model);
        Task<bool> UpdateAsync(Hra model);
        Task<bool> DeleteAsync(int id);
        Task<Hra> GetByName(string name);
    }
}
