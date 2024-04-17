using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turnaje.Database.Entity;

namespace Turnaje.Database.Repository
{
    public class HraciRepository : IUHraciRepository
    {
        public Task<int> AddAsync(Hraci model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Hraci>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Hraci> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Hraci model)
        {
            throw new NotImplementedException();
        }
    }
}
