using Business.Core;
using Entity.Entities;
using Shopping.Business.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface ICategoryService
    {
        public Task<CategoryDto> Create(CategoryResponse res);

        public Task<BaseDto> Delete(Guid Id);

        public Task<CategoryListDto> GetAll(int page);

        public Task<CategoryDto> Get(Guid Id);

        public Task<CategoryDto> Update(Guid Id, CategoryResponse res);
        
    }
}
