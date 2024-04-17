using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public interface IUHraciRepository
    {
        Task<IEnumerable<Hraci>> GetAllAsync();
        Task<Hraci> GetByIdAsync(int id);
        Task<int> AddAsync(Hraci model);
        Task<bool> UpdateAsync(Hraci model);
        Task<bool> DeleteAsync(int id);
    }
}
