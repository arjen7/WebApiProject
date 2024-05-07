using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Interface
{
    public interface IUserRepository:IBaseRepository<User>
    {
        public  Task<User> GetByEmailAsync(string Email);
    }
}
