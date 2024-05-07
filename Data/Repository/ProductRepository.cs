using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Shopping.Data.Context;
using Shopping.Data.Interface;

namespace Shopping.Data.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ShoppingDbContext DbContext) : base(DbContext)
        {
        }

        public async Task<ICollection<Product>> GetAllAsync(int page, Guid? categoryid, string keyword)
        {
            var products= await _dbContext.Products.Where(t=>t.Name.StartsWith(keyword)).ToListAsync();
            if (categoryid.HasValue)
            {
                products=products.Where(t=>t.CategoryId==categoryid.Value).ToList();
            }
            return products.Skip(page*50).Take(50).ToList();
        }
    }
}
