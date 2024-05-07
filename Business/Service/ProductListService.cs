using Business.Core;
using Entity.Entities;
using Shopping.Business.Core;
using Shopping.Data.Interface;

namespace Business.Service
{
    public class ProductListService: Interface.IProductListService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductListRepository _productListRepository;

        public ProductListService(IProductListRepository productListRepository, IUserRepository userRepository)
        {
            _productListRepository = productListRepository;
            _userRepository = userRepository;
        }

        public async Task<ProductListDto> Create(Guid UserId, ProductListResponse res)
        {
            Guid id = Guid.NewGuid();
            var response = new ProductListDto();
            var user = await _userRepository.GetByIdAsync(UserId);
            if (user == null)
            {
                response.DeveloperMessage = "User Not Found";
                response.Message = "Not Found";
                response.Status = 404;
            }
            
            while (await _productListRepository.GetByIdAsync(id) != null)
            {
                id = Guid.NewGuid();
            }

            var newproductlist = new ProductList()
            {
                Id = id,
                Name=res.Name,
                UserId=UserId,
            };
            await _productListRepository.AddAsync(newproductlist);
            response.DeveloperMessage = "Product List Created Succesfull";
            response.ProductList = newproductlist;
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        public async Task<BaseDto> Delete(Guid UserId, Guid Id)
        {
            var productlist = await _productListRepository.GetByIdAsync(Id);
            var response = new BaseDto();
            if (productlist == null)
            {
                response.DeveloperMessage = "Product List Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            if (UserId != productlist.UserId)
            {
                response.DeveloperMessage = "Product List Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            await _productListRepository.DeleteAsync(Id);
            response.DeveloperMessage = "Product List removed successful";
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        public async Task<ProdutAllListDto> GetAll(Guid UserId, int page)
        {
            var products = await _productListRepository.GetAllAsync(UserId, page);
            var res = new ProdutAllListDto()
            {

                Message = "Success",
                Status = 200,
                ProductLists = products
            };
            return res;
        }

        public async Task<ProductListDto> Get(Guid UserId, Guid Id)
        {
            var response = new ProductListDto();
            var product = await _productListRepository.GetByIdAsync(UserId, Id);
            if (product == null)
            {
                response.DeveloperMessage = "Product List Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            response.Message = "Success";
            response.ProductList = product;
            response.Status = 200;
            return response;
        }

        public async Task<BaseDto> Update(Guid UserId, Guid Id,bool IsComplete)
        {
            var response = new BaseDto();
            var productlist = await _productListRepository.GetByIdAsync(UserId, Id);
            if (productlist == null)
            {
                response.DeveloperMessage = "Product List Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            productlist.IsCompleted = IsComplete;
            await _productListRepository.UpdateAsync(Id, productlist);
            response.Message = "Success";
            response.Status = 200;
            return response;
        }
    }
}
