using Entity.Entities;
using Shopping.Data.Context;
using Shopping.Data.Interface;

namespace Shopping.Data.Repository
{
    public class CategoryRepository : BaseRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(ShoppingDbContext DbContext) : base(DbContext)
        {
        }
    }
}
