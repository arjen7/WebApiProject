using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core
{
    public class BaseDto
    {
            public int Status { get; set; }

            public string Message { get; set; }=string.Empty;

            public string DeveloperMessage { get; set; } = string.Empty;
    }
    public class TokenDto:BaseDto
    {
        public string? AuthToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
    }
    public class UserDto : BaseDto
    {
        public User User { get; set; }
    }
    public class UserListDto : BaseDto
    {
        public ICollection<User> Users { get; set; }
    }
    public class CategoryListDto : BaseDto
    {
        public ICollection<Category> Categories { get; set; }
    }
    public class CategoryDto : BaseDto
    {
        public Category Category { get; set; }
    }
    public class ProductDto : BaseDto
    {
        public Product Product { get; set; }
    }
    public class ProductAllDto : BaseDto
    {
        public ICollection<Product> Products { get; set; }
    }

    public class UserProductDto : BaseDto
    {
        public UserProduct UserProduct { get; set; }
    }
    public class UserProductListDto : BaseDto
    {
        public ICollection<UserProduct> UserProducts { get; set; }
    }
    public class ProductListDto : BaseDto
    {
        public ProductList ProductList { get; set; }
    }
    public class ProdutAllListDto : BaseDto
    {
        public ICollection<ProductList> ProductLists { get; set; }
    }
}
