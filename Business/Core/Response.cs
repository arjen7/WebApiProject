using Microsoft.AspNetCore.Http;

namespace Shopping.Business.Core
{
    
    public class UserSignupResponse
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PasswordAgain { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
    }
    public class UserLoginResponse
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class UserChangePasswordResponse
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
    public class UserProductResponse
    {
        public Guid ProductListId { get; set; }
        public Guid Productid { get; set; }
        public string Comment { get; set; } = "";
    }
    public class UserProductUpdateResponse
    {
        public string? Comment { get; set; } = "";
        public bool IsTake {  get; set; }
    }
    public class ProductListResponse
    {
        public string Name { get; set; }
    }
    public class UserUpdateResponse
    {
        public  string? Email { get; set; }
        public  string? Name { get; set; }
        public  string? LastName { get; set; }
    }
    public class UserDeleteResponse
    {
        public string password { get; set; }
    }
    public class CategoryResponse
    {
        public string Name { get; set; }    
    }

    public class ProductResponse
    {
        public IFormFile Image { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class ProductUpdateResponse
    {
        public IFormFile? Image { get; set; }
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
