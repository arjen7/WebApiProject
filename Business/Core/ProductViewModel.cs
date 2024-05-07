using Microsoft.AspNetCore.Http;

namespace Shopping.Business.Core
{
    public class ProductViewModel
    {
        public  IFormFile? Image { get; set; }
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
