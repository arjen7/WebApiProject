using Entity.Abstract;

namespace Entity.Entities
{
    public class Category : BaseModel
    {
        public  string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
