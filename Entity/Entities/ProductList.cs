using Entity.Abstract;

namespace Entity.Entities
{
    public class ProductList : BaseModel
    {
        
        public  string Name { get; set; }
        public Guid UserId { get; set; }
        public  User User { get; set; }
        public ICollection<UserProduct> UserProducts { get; set; }
        public bool IsCompleted { get; set; }
    }
}
