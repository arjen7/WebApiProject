using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Interface
{
    public interface IProductListRepository:IBaseRepository<ProductList>
    {
        public Task<ICollection<ProductList>> GetAllAsync(Guid UserId, int page);

        public Task<ProductList> GetByIdAsync(Guid UserId, Guid id);
        
    }
}
