using Business.Core;
using Shopping.Business.Core;

namespace Shopping.Interface
{
    public interface IUserProductService
    {
        public Task<UserProductDto> Create(Guid UserId, UserProductResponse res);
        public Task<BaseDto> Delete(Guid UserId, Guid Id);
        public Task<UserProductListDto> GetAll(Guid UserId, int page);
        public Task<UserProductDto> Get(Guid UserId, Guid Id);
        
    }
}
