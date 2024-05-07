using Entity.Abstract;

namespace Entity.Entities
{
    public class UserProduct : BaseModel
    {
        public string Comment { get; set; }
        public Guid ProductListId { get; set; }
        public  ProductList ProductList { get; set; }
        public Guid ProductId { get; set; }
        public  Product Product { get; set; }
    }
}
