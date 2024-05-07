using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Interface
{
    public interface IUserProductRepository:IBaseRepository<UserProduct>
    {
        public Task<ICollection<UserProduct>> GetAllAsync(Guid UserId, int page);
        public  Task<UserProduct> GetByIdAsync(Guid UserId, Guid id);
    }
}
