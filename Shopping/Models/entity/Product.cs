using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.entity
{
    public class Product : BaseModel
    {
        public Product()
        {
            UserProducts = new HashSet<UserProduct>();
        }
        [MaxLength(50)] public required string Name { get; set; }
        [MaxLength(1000)] public required string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public required Category Category { get; set; }
        public ICollection<UserProduct> UserProducts { get; set; }
    }
}
