using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.entity
{
    public class ProductList : BaseModel
    {
        public ProductList()
        {
            UserProducts = new HashSet<UserProduct>();
        }
        [MaxLength(50)] public required string Name { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public ICollection<UserProduct> UserProducts { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
