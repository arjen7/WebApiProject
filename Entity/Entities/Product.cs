using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Entity.Abstract;

namespace Entity.Entities
{
    public class Product : BaseModel
    {
        
        public  string Name { get; set; }
        public  string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public  Category Category { get; set; }
        public ICollection<UserProduct> UserProducts { get; set; }
    }
}
