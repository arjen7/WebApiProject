namespace Shopping.Models
{
    public class ProductViewModel
    {
        public  IFormFile? Image { get; set; }
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
    }
}
