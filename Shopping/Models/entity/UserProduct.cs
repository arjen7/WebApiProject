using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.entity
{
    public class UserProduct : BaseModel
    {
        [MaxLength(50)] public string Comment { get; set; } = "";
        
        public int ProductListId { get; set; }
        public required ProductList ProductList { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}
