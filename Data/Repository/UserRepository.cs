using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Shopping.Data.Context;
using Shopping.Data.Interface;

namespace Shopping.Data.Repository
{
    public class UserRepository : BaseRepository<User>,IUserRepository
    {
        public UserRepository(ShoppingDbContext DbContext) : base(DbContext)
        {
        }

        public async Task<User> GetByEmailAsync(string Email)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(t=>t.Email==Email);
        }
    }
}
