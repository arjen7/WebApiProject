using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Interface
{
    public interface IProductRepository:IBaseRepository<Product>
    {
        Task<ICollection<Product>> GetAllAsync(int page, Guid? categoryid, string keyword);
    }
}
