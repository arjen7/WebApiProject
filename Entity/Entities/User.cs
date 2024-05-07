
using Entity.Abstract;

namespace Entity.Entities
{
    public class User : BaseModel
    {
        
        public  string Email { get; set; }
        public  string Name { get; set; }
        public  string LastName { get; set; }

        public  string Password { get; set; }
        public string Role { get; set; } = "User";
        public ICollection<ProductList> ProductLists { get; set; }
    }
}
