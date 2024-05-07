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
    public interface IProductListService
    {
        public Task<ProductListDto> Create(Guid UserId, ProductListResponse res);
        public Task<BaseDto> Delete(Guid UserId, Guid Id);
        public Task<ProdutAllListDto> GetAll(Guid UserId, int page);
        public Task<ProductListDto> Get(Guid UserId, Guid Id);
        public Task<BaseDto> Update(Guid UserId, Guid Id, bool IsComplete);
        
    }
}
