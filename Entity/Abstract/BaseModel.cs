namespace Entity.Abstract
{
    public abstract class BaseModel 
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
