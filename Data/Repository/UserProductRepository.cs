using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Shopping.Data.Context;
using Shopping.Data.Interface;

namespace Shopping.Data.Repository
{
    public class UserProductRepository : BaseRepository<UserProduct>, IUserProductRepository
    {
        public UserProductRepository(ShoppingDbContext DbContext) : base(DbContext)
        {
        }
        public async Task<ICollection<UserProduct>> GetAllAsync(Guid UserId,int page)
        {
            IQueryable<UserProduct> query = _dbContext.Set<UserProduct>().Include(t=>t.Product).Where(t=>t.ProductList.UserId==UserId).Skip(50 * page).Take(50).AsNoTrackingWithIdentityResolution();

            return await query.ToListAsync();
        }
        public async Task<UserProduct> GetByIdAsync(Guid UserId,Guid id)
        {
            return await _dbContext.Set<UserProduct>().Include(t=>t.Product).Where(t=>t.ProductList.UserId==UserId).SingleOrDefaultAsync(t=>t.Id==id);
        }
    }
}
