using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.entity
{
    public class Category : BaseModel
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [MaxLength(50)] public required string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; }
    }
}
