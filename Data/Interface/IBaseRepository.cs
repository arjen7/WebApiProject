using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<ICollection<TEntity>> GetAllAsync(int Page);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(Guid id, TEntity entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
