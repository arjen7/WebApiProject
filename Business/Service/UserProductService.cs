using Business.Core;
using Entity.Entities;
using Microsoft.IdentityModel.Tokens;
using Shopping.Business.Core;
using Shopping.Data.Interface;
using Shopping.Interface;

namespace Business.Service
{
    public class UserProductService:IUserProductService
    {
        private readonly IUserProductRepository _userProductRepository;
        private readonly IProductListRepository _productListRepository;
        private readonly IUserRepository _userRepository;

        public UserProductService(IUserProductRepository userProductRepository, IProductListRepository productListRepository, IUserRepository userRepository)
        {
            _userProductRepository = userProductRepository;
            _productListRepository = productListRepository;
            _userRepository = userRepository;
        }

        public async Task<UserProductDto> Create(Guid UserId,UserProductResponse res)
        {
            Guid id = Guid.NewGuid();
            var response = new UserProductDto();
            var user=await _userRepository.GetByIdAsync(UserId);
            var product = await _userProductRepository.GetByIdAsync(res.Productid);
            var productlist = await _productListRepository.GetByIdAsync(res.ProductListId);
            if (user == null)
            {
                response.DeveloperMessage = "User Not Found";
                response.Message = "Not Found";
                response.Status = 404;
            }
            if (product == null)
            {
                response.DeveloperMessage = "Product Not Found";
                response.Message = "Not Found";
                response.Status = 404;
            }
            if (productlist == null || productlist.UserId!=user.Id)
            {
                response.DeveloperMessage = "ProductList Not Found";
                response.Message = "Not Found";
                response.Status = 404;
            }
            if(productlist.IsCompleted)
            {
                response.DeveloperMessage = "ProductList Is Completed";
                response.Message = "Bad Request";
                response.Status = 400;
            }
            while (await _userProductRepository.GetByIdAsync(id) != null)
            {
                id = Guid.NewGuid();
            }
            
            var newproduct = new UserProduct
            {
                Id = id,
                Comment= res.Comment,
                ProductListId = res.ProductListId,
                ProductId=res.Productid,
            };
            await _userProductRepository.AddAsync(newproduct);
            response.DeveloperMessage = "User Product Created Succesfull";
            response.UserProduct = newproduct;
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        public async Task<BaseDto> Delete(Guid UserId,Guid Id)
        {
            var product = await _userProductRepository.GetByIdAsync(Id);
            var response = new BaseDto();
            if (product == null)
            {
                response.DeveloperMessage = "User Product Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            var productlist=await _productListRepository.GetByIdAsync(product.ProductListId);
            if(productlist==null||UserId!=productlist.UserId)
            {
                response.DeveloperMessage = "User Product Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            await _userProductRepository.DeleteAsync(Id);
            response.DeveloperMessage = "User Product removed successful";
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        public async Task<UserProductListDto> GetAll(Guid UserId,int page)
        {
            var products = await _userProductRepository.GetAllAsync(UserId,page);
            var res = new UserProductListDto()
            {

                Message = "Success",
                Status = 200,
                UserProducts = products
            };
            return res;
        }

        public async Task<UserProductDto> Get(Guid UserId,Guid Id)
        {
            var response = new UserProductDto();
            var product = await _userProductRepository.GetByIdAsync(UserId,Id);
            if (product == null)
            {
                response.DeveloperMessage = "userproduct Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            response.Message = "Success";
            response.UserProduct = product;
            response.Status = 200;
            return response;
        }
        public async Task<UserProductDto> Update(Guid UserId,Guid Id,UserProductUpdateResponse res )
        {
            var response = new UserProductDto();
            var product = await _userProductRepository.GetByIdAsync(UserId, Id);
            if (product == null)
            {
                response.DeveloperMessage = "userproduct Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            if (!res.Comment.IsNullOrEmpty())
            {
                product.Comment = res.Comment;
            }
            product.IsTake = res.IsTake;
            await _userProductRepository.UpdateAsync(Id, product);
            response.Message = "Success";
            response.DeveloperMessage = "UserProduct Update";
            response.UserProduct = product;
            response.Status = 200;
            return response;
        }


    }
}
